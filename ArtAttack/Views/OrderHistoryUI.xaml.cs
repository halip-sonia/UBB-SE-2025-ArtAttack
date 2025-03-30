using ArtAttack.Domain;
using ArtAttack.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        Dictionary<int, string> productNames = new Dictionary<int, string>();
        Dictionary<int, string> productTypeNames = new Dictionary<int, string>();

        var selectedPeriod = (TimePeriodComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"SELECT 
                o.OrderID, 
                p.name AS ProductName, 
                o.ProductType,  
                p.productType AS ProductTypeName,  
                o.OrderDate, 
                o.PaymentMethod, 
                o.OrderSummaryID
            FROM [Order] o
            JOIN [DummyProduct] p ON o.ProductType = p.ID
            WHERE o.BuyerID = @UserId";

            if (!string.IsNullOrEmpty(searchText))
                query += " AND p.name LIKE @SearchText";

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
                    command.Parameters.AddWithValue("@SearchText", $"%{searchText}%");

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var order = new Order
                        {
                            OrderID = reader.GetInt32(0),
                            ProductType = reader.GetInt32(2),
                           
                            OrderDate = reader.GetDateTime(4),
                            PaymentMethod = reader.GetString(5),
                            OrderSummaryID = reader.GetInt32(6)
                        };

                        orders.Add(order);
                        productNames[order.OrderID] = reader.GetString(1); 
                        productTypeNames[order.OrderID] = reader.GetString(3);
                    }
                }
            }
        }

        // Update UI based on results
        if (orders.Count > 0)
        {
            var displayOrders = orders.Select(o => new
            {
                o.OrderID,
                ProductName = productNames.GetValueOrDefault(o.OrderID, "Unknown"), // Safe lookup
                //o.ProductType,
                ProductTypeName = productTypeNames.GetValueOrDefault(o.OrderID, "Unknown"),
                OrderDate = o.OrderDate.ToString("yyyy-MM-dd"),
                o.PaymentMethod,
                o.OrderSummaryID
            }).ToList();

            OrdersListView.ItemsSource = displayOrders;

            OrdersListView.Visibility = Visibility.Visible;
            NoResultsText.Visibility = Visibility.Collapsed;
        }
        else
        {
            OrdersListView.Visibility = Visibility.Collapsed;
            NoResultsText.Visibility = Visibility.Visible;

            // Customize message based on filters
            NoResultsText.Text = string.IsNullOrEmpty(searchText) ? "No orders found" : $"No orders found containing '{searchText}'";
            if (selectedPeriod != "All Orders")
                NoResultsText.Text += $" in {selectedPeriod}";
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