using ArtAttack.Domain;
using ArtAttack.ViewModel;
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

        private async void RefreshButton_Click(object sender, RoutedEventArgs e) => await LoadOrders();

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