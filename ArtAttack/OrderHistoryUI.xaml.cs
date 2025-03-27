using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using ArtAttack.Domain;

namespace ArtAttack
{
    public sealed partial class OrderHistoryUI : Window
    {
        private readonly string _connectionString;
        private readonly int _userId;

        public OrderHistoryUI(string connectionString, int userId)
        {
            this.InitializeComponent();
            _connectionString = connectionString;
            _userId = userId;
            this.Activated += OrderHistoryUI_Activated;
        }

        private async void OrderHistoryUI_Activated(object sender, WindowActivatedEventArgs args)
        {
            this.Activated -= OrderHistoryUI_Activated;
            await LoadOrders();
        }

        private async Task LoadOrders()
        {
            try
            {
                List<Order> orders = new List<Order>();
                var selectedPeriod = (TimePeriodComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"SELECT OrderID, ProductType, OrderDate, PaymentMethod 
                                    FROM [Order]
                                    WHERE BuyerID = @UserId";

                    // Add time filter
                    if (selectedPeriod == "Last 3 Months")
                        query += " AND OrderDate >= DATEADD(month, -3, GETDATE())";
                    else if (selectedPeriod == "Last 6 Months")
                        query += " AND OrderDate >= DATEADD(month, -6, GETDATE())";
                    else if (selectedPeriod == "This Year")
                        query += " AND YEAR(OrderDate) = YEAR(GETDATE())";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", _userId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                orders.Add(new Order
                                {
                                    OrderID = reader.GetInt32(0),
                                    ProductType = reader.GetInt32(1),
                                    OrderDate = reader.GetDateTime(2),
                                    PaymentMethod = reader.GetString(3)
                                });
                            }
                        }
                    }
                }

                OrdersListView.ItemsSource = orders;
            }
            catch (Exception ex)
            {
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
            await LoadOrders();
        }
    }
}