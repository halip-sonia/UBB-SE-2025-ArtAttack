using ArtAttack.Domain;
using ArtAttack.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ArtAttack.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TrackedOrderControlPage : Page
    {
        private readonly ITrackedOrderViewModel viewModel;
        public int TrackedOrderID { get; set; }
        internal List<OrderCheckpoint> Checkpoints { get; set; }

        public TrackedOrderControlPage(int trackedOrderID)
        {
            this.InitializeComponent();
            viewModel = new TrackedOrderViewModel(Shared.Configuration._CONNECTION_STRING_);
            TrackedOrderID = trackedOrderID;
            Checkpoints = new List<OrderCheckpoint>();
            LoadOrderData();
        }

        private async void LoadOrderData()
        {
            var order = await viewModel.GetTrackedOrderByIDAsync(TrackedOrderID);
            if (order != null)
            {
                var checkpoints = await viewModel.GetAllOrderCheckpointsAsync(TrackedOrderID);
                checkpoints.Reverse();

                DataContext = new
                {
                    CurrentStatus = order.CurrentStatus,
                    TrackedOrderID = order.TrackedOrderID,
                    DeliveryAddress = order.DeliveryAddress,
                    EstimatedDeliveryDate = order.EstimatedDeliveryDate,
                    Checkpoints = checkpoints
                };
            }
        }

        private async Task ShowErrorDialog(string errorMessage)
        {
            var dialog = new ContentDialog
            {
                Title = "Error",
                Content = errorMessage,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }

        private async void RevertLastCheckpointButton_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var order = await viewModel.GetTrackedOrderByIDAsync(TrackedOrderID);
                if (order == null)
                    throw new Exception("No TrackedOrder corresponds to the id: " + TrackedOrderID.ToString());
                await viewModel.RevertToPreviousCheckpoint((TrackedOrder)order);
                LoadOrderData();
            }
            catch (Exception ex)
            {
                await ShowErrorDialog($"{ex.Message}");
            }
        }

        private void ChangeEstimatedDeliveryDateButton_Clicked(object sender, RoutedEventArgs e)
        {
            if(deliveryCalendarDatePicker.Visibility == Visibility.Collapsed)
            {
                deliveryCalendarDatePicker.Visibility = Visibility.Visible;
                deliveryCalendarDatePicker.MinDate = DateTime.Now;
                confirmChangeEstimatedDeliveryDateButton.Visibility = Visibility.Visible;
            }
            else
            {
                deliveryCalendarDatePicker.Visibility = Visibility.Collapsed;
                confirmChangeEstimatedDeliveryDateButton.Visibility = Visibility.Collapsed;
            }
        }

        private async void ConfirmChangeEstimatedDeliveryDateButton_Clicked(object sender, RoutedEventArgs e)
        {
            var pickedDate = deliveryCalendarDatePicker.Date;
            if(pickedDate!=null)
            {
                try
                {
                    DateTime pickedDateTime = pickedDate.Value.DateTime;
                    DateOnly newEstimatedDeliveryDate = DateOnly.FromDateTime(pickedDateTime);
                    var order = await viewModel.GetTrackedOrderByIDAsync(TrackedOrderID);
                    if (order == null)
                        throw new Exception("No TrackedOrder corresponds to the id: " + TrackedOrderID.ToString());
                    await viewModel.UpdateTrackedOrderAsync(TrackedOrderID, newEstimatedDeliveryDate, order.CurrentStatus);
                    LoadOrderData();
                }
                catch (Exception ex)
                {
                    await ShowErrorDialog($"{ex.Message}");
                }
            }
        }
    }
}
