using ArtAttack.Domain;
using ArtAttack.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtAttack
{
    public sealed partial class OrderHistoryUI : Window
    {
        private readonly IOrderViewModel _orderViewModel;
        private readonly int _currentUserId;

        public OrderHistoryUI(string connectionString, int userId)
        {
            this.InitializeComponent();
            _orderViewModel = new OrderViewModel(connectionString);
            _currentUserId = userId;

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
                string timePeriod = GetSelectedTimePeriod();
                List<Order> orders = await LoadFilteredOrders(timePeriod);
                OrdersListView.ItemsSource = orders;
            }
            catch (Exception ex)
            {
                await ShowMessageAsync("Error", $"Failed to load orders: {ex.Message}");
            }
        }

        private async Task<List<Order>> LoadFilteredOrders(string timePeriod)
        {
            return timePeriod.ToLower() switch
            {
                "3months" => await _orderViewModel.GetOrdersFromLastThreeMonthsAsync(_currentUserId),
                "6months" => await _orderViewModel.GetOrdersFromLastSixMonthsAsync(_currentUserId),
                "2024" => await _orderViewModel.GetOrdersFrom2024Async(_currentUserId),
                "2025" => await _orderViewModel.GetOrdersFrom2025Async(_currentUserId),
                _ => await GetAllOrdersAsync()
            };
        }

        private async Task<List<Order>> GetAllOrdersAsync()
        {
            var borrowed = await _orderViewModel.GetBorrowedOrderHistoryAsync(_currentUserId);
            var newUsed = await _orderViewModel.GetNewOrUsedOrderHistoryAsync(_currentUserId);
            return borrowed.Concat(newUsed).ToList();
        }

        private string GetSelectedTimePeriod()
        {
            return TimePeriodComboBox.SelectedIndex switch
            {
                0 => "3months",
                1 => "6months",
                2 => DateTime.Now.Year.ToString(),
                _ => "all"
            };
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadOrders();
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

        private async void OrdersListView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (OrdersListView.SelectedItem is Order selectedOrder)
            {
                try
                {
                    var summary = await _orderViewModel.GetOrderSummaryAsync(selectedOrder.OrderSummaryID);
                    if (summary != null)
                    {
                        string details = $"Order Summary Details:\n\n" +
                                      $"Subtotal: {summary.Subtotal:C}\n" +
                                      $"Warranty Tax: {summary.WarrantyTax:C}\n" +
                                      $"Delivery Fee: {summary.DeliveryFee:C}\n" +
                                      $"Final Total: {summary.FinalTotal:C}\n\n" +
                                      $"Customer Info:\n" +
                                      $"Name: {summary.FullName}\n" +
                                      $"Email: {summary.Email}\n" +
                                      $"Phone: {summary.PhoneNumber}\n\n" +
                                      $"Shipping Address:\n" +
                                      $"{summary.Address}\n" +
                                      $"{summary.PostalCode}";

                        await ShowMessageAsync($"Order #{selectedOrder.OrderID} Details", details);
                    }
                }
                catch (Exception ex)
                {
                    await ShowMessageAsync("Error", $"Failed to load order details: {ex.Message}");
                }
            }
        }
    }
}