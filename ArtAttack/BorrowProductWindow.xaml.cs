using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ArtAttack.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using System.Data;
using ArtAttack.Services;

namespace ArtAttack
{
    public sealed partial class BorrowProductWindow : Window
    {
        private readonly string _connectionString;
        private readonly int _currentProductId;
        private readonly WaitListViewModel _waitListViewModel;

        public BorrowProductWindow(string connectionString, int productId)
        {
            InitializeComponent();
            _connectionString = connectionString;
            _currentProductId = productId;
            _waitListViewModel = new WaitListViewModel(connectionString);
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
                var product = await _waitListViewModel.GetDummyProductByIdAsync(_currentProductId);
                if (product != null)
                {
                    string sellerName = await _waitListViewModel.GetSellerNameAsync(product.SellerID);
                    DisplayProduct(product, sellerName);

                    int currentUserId = GetCurrentUserId();
                    bool isOnWaitlist = _waitListViewModel.IsUserInWaitlist(currentUserId, _currentProductId);

                    // Update UI based on waitlist status
                    UpdateWaitlistUI(isOnWaitlist);
                }
                else
                {
                    await ShowMessageAsync("Error", "Product not found");
                }
            }
            catch (Exception ex)
            {
                await ShowMessageAsync("Error", $"Failed to load product: {ex.Message}");
            }
        }

        private void UpdateWaitlistUI(bool isOnWaitlist)
        {
            btnJoinWaitList.Visibility = isOnWaitlist ? Visibility.Collapsed : Visibility.Visible;
            waitlistActionsPanel.Visibility = isOnWaitlist ? Visibility.Visible : Visibility.Collapsed;

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

        private async void btnJoinWaitList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int currentUserId = GetCurrentUserId();

                if (_waitListViewModel.IsUserInWaitlist(currentUserId, _currentProductId))
                {
                    await ShowMessageAsync("Waitlist", "You're already on the waitlist for this product");
                    return;
                }

                _waitListViewModel.AddUserToWaitlist(currentUserId, _currentProductId);

                btnJoinWaitList.IsEnabled = false;
                btnJoinWaitList.Content = "On Waitlist";

                await ShowMessageAsync("Success", "You've been added to the waitlist!");
            }
            catch (Exception ex)
            {
                await ShowMessageAsync("Error", $"Failed to join waitlist: {ex.Message}");
            }
        }

        private int GetCurrentUserId()
        {
            return 1;
        }

        private async void btnLeaveWaitList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int currentUserId = GetCurrentUserId();

                // Remove user from waitlist
                _waitListViewModel.RemoveUserFromWaitlist(currentUserId, _currentProductId);

                // Update UI to show join button and hide leave/position buttons
                UpdateWaitlistUI(false);

                // Hide position text
                await ShowMessageAsync("Success", "You've left the waitlist");
            }
            catch (Exception ex)
            {
                await ShowMessageAsync("Error", $"Failed to leave waitlist: {ex.Message}");
            }

        }

        private void btnViewPosition_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for future implementation
            // Will show queue position when implemented
        }

    }
}