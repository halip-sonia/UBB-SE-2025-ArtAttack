
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using ArtAttack.Domain;
using ArtAttack.ViewModel;
using Microsoft.UI.Text;

namespace ArtAttack
{
    public sealed partial class OrderHistoryUI : Window
    {
        private readonly string _connectionString;
        private readonly int _userId;
        private OrderViewModel _orderViewModel;

       
        public OrderHistoryUI(string connectionString, int userId)
        {
            InitializeComponent();
            _connectionString = connectionString;
            _userId = userId;
            _orderViewModel = new OrderViewModel(connectionString);
            this.Activated += Window_Activated;
        }

        private async void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            this.Activated -= Window_Activated;
            await LoadOrders(SearchTextBox.Text);
        }

        private async Task LoadOrders(string searchText = null)
        {
            try
            {
                List<Order> orders = new List<Order>();
                var selectedPeriod = (TimePeriodComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"SELECT o.OrderID, p.name AS ProductName, o.OrderDate, 
                          o.PaymentMethod, o.OrderSummaryID
                          FROM [Order] o
                          JOIN [DummyProduct] p ON o.ProductType = p.ID
                          WHERE o.BuyerID = @UserId";

                    // Add search filter if text is provided
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        query += " AND p.name LIKE @SearchText";
                    }

                    // Add time filter
                    if (selectedPeriod == "Last 3 Months")
                        query += " AND o.OrderDate >= DATEADD(month, -3, GETDATE())";
                    else if (selectedPeriod == "Last 6 Months")
                        query += " AND o.OrderDate >= DATEADD(month, -6, GETDATE())";
                    else if (selectedPeriod == "This Year")
                        query += " AND YEAR(o.OrderDate) = YEAR(GETDATE())";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", _userId);

                        if (!string.IsNullOrEmpty(searchText))
                        {
                            command.Parameters.AddWithValue("@SearchText", $"%{searchText}%");
                        }

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                orders.Add(new Order
                                {
                                    OrderID = reader.GetInt32(0),
                                    ProductName = reader.GetString(1),
                                    OrderDate = reader.GetDateTime(2),
                                    PaymentMethod = reader.GetString(3),
                                    OrderSummaryID = reader.GetInt32(4)
                                });
                            }
                        }
                    }
                }

                // Update UI based on results
                if (orders.Count > 0)
                {
                    OrdersListView.ItemsSource = orders;
                    OrdersListView.Visibility = Visibility.Visible;
                    NoResultsText.Visibility = Visibility.Collapsed;
                }
                else
                {
                    OrdersListView.Visibility = Visibility.Collapsed;
                    NoResultsText.Visibility = Visibility.Visible;

                    // Customize message based on filters
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        NoResultsText.Text = $"No orders found containing '{searchText}'";
                        if (selectedPeriod != "All Orders")
                        {
                            NoResultsText.Text += $" in {selectedPeriod}";
                        }
                    }
                    else if (selectedPeriod != "All Orders")
                    {
                        NoResultsText.Text = $"No orders found in {selectedPeriod}";
                    }
                    else
                    {
                        NoResultsText.Text = "No orders found";
                    }
                }
            }
            catch (Exception ex)
            {
                OrdersListView.Visibility = Visibility.Collapsed;
                NoResultsText.Visibility = Visibility.Visible;
                NoResultsText.Text = "Error loading orders";

                var dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"Failed to load orders: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
            }
        }
        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadOrders(SearchTextBox.Text);
        }

        private async void OrderDetails_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int orderSummaryId)
            {
                try
                {
                    OrderDetailsDialog.XamlRoot = this.Content.XamlRoot;
                    // Show loading indicator
                    button.Content = "Loading...";
                    button.IsEnabled = false;

                    var orderSummary = await _orderViewModel.GetOrderSummaryAsync(orderSummaryId);

                    // Clear previous content
                    OrderDetailsContent.Children.Clear();

                    // Add order details to the dialog
                    AddDetailRow("Order Summary ID:", orderSummary.ID.ToString());
                    AddDetailRow("Subtotal:", orderSummary.Subtotal.ToString("C"));
                    AddDetailRow("Warranty Tax:", orderSummary.WarrantyTax.ToString("C"));
                    AddDetailRow("Delivery Fee:", orderSummary.DeliveryFee.ToString("C"));
                    AddDetailRow("Final Total:", orderSummary.FinalTotal.ToString("C"));
                    AddDetailRow("Customer Name:", orderSummary.FullName);
                    AddDetailRow("Email:", orderSummary.Email);
                    AddDetailRow("Phone:", orderSummary.PhoneNumber);
                    AddDetailRow("Address:", orderSummary.Address);
                    AddDetailRow("Postal Code:", orderSummary.PostalCode);

                    if (!string.IsNullOrEmpty(orderSummary.AdditionalInfo))
                        AddDetailRow("Additional Info:", orderSummary.AdditionalInfo);

                    if (!string.IsNullOrEmpty(orderSummary.ContractDetails))
                        AddDetailRow("Contract Details:", orderSummary.ContractDetails);

                    // Show the dialog
                    await OrderDetailsDialog.ShowAsync();
                }
                catch (KeyNotFoundException ex)
                {
                    await ShowMessageAsync("Error", ex.Message);
                }
                catch (Exception ex)
                {
                    await ShowMessageAsync("Error", $"Failed to load order details: {ex.Message}");
                }
                finally
                {
                    // Reset button state
                    button.Content = "See Details";
                    button.IsEnabled = true;
                }
            }
        }

        private async Task ShowMessageAsync(string title, string message)
        {
            // Ensure we have a valid XAML root
            if (this.Content.XamlRoot == null) return;

            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot,
                
            };

            await dialog.ShowAsync();
        }
        private void AddDetailRow(string label, string value)
        {
            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };
            stackPanel.Children.Add(new TextBlock { Text = label, FontWeight = FontWeights.SemiBold, Width = 150 });
            stackPanel.Children.Add(new TextBlock { Text = value });
            OrderDetailsContent.Children.Add(stackPanel);
        }

        private async void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Add slight delay to avoid searching on every keystroke
            await Task.Delay(300); // 300ms delay
            await LoadOrders(SearchTextBox.Text);
        }
    }
}