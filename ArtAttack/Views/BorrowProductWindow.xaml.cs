using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ArtAttack.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using System.Data;

namespace ArtAttack
{
    public sealed partial class BorrowProductWindow : Window
    {
        private readonly string _connectionString;
        private readonly int _currentProductId;

        public BorrowProductWindow(string connectionString, int productId)
        {
            InitializeComponent();
            _connectionString = connectionString;
            _currentProductId = productId;
            this.Activated += Window_Activated;
        }

        private async void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            this.Activated -= Window_Activated;
            await LoadProductDetails();
        }

        private async Task LoadProductDetails()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    // Get product and seller info
                    var query = @"
                        SELECT p.*, s.name as SellerName 
                        FROM DummyProduct p
                        LEFT JOIN DummySeller s ON p.SellerID = s.ID
                        WHERE p.ID = @ProductId";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", _currentProductId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var product = new DummyProduct
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    Name = reader.GetString(reader.GetOrdinal("name")),
                                    Price = (float)reader.GetDouble(reader.GetOrdinal("price")),
                                    SellerID = reader.IsDBNull("SellerID") ? 0 : reader.GetInt32("SellerID"),
                                    ProductType = reader.GetString(reader.GetOrdinal("productType")),
                                    StartDate = reader.IsDBNull("startDate") ? DateTime.MinValue : reader.GetDateTime("startDate"),
                                    EndDate = reader.IsDBNull("endDate") ? DateTime.MinValue : reader.GetDateTime("endDate")
                                };

                                string sellerName = reader.IsDBNull("SellerName") ? "No Seller" : reader.GetString("SellerName");
                                DisplayProduct(product, sellerName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await ShowMessageAsync("Error", $"Failed to load product: {ex.Message}");
            }
        }

        private void DisplayProduct(DummyProduct product, string sellerName)
        {
            txtProductName.Text = product.Name;
            txtPrice.Text = $"Price: ${product.Price}";
            txtSeller.Text = $"Seller: {sellerName}";
            txtType.Text = $"Type: {product.ProductType}";

            bool isAvailable = product.EndDate == DateTime.MinValue;

            if (isAvailable)
            {
                txtDates.Text = product.StartDate == DateTime.MinValue
                    ? "Availability: Now"
                    : $"Available after: {product.StartDate:yyyy-MM-dd}";

                btnBorrow.Visibility = Visibility.Visible;
                btnJoinWaitList.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtDates.Text = $"Unavailable until: {product.EndDate:yyyy-MM-dd}";
                btnBorrow.Visibility = Visibility.Collapsed;
                btnJoinWaitList.Visibility = Visibility.Visible;
            }
        }

        private async Task ShowMessageAsync(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }
}