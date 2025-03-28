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
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ArtAttack.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TrackedOrderBuyerPage : Page
    {
        private readonly TrackedOrderViewModel viewModel;
        public int TrackedOrderID { get; set; }
        internal List<OrderCheckpoint> Checkpoints { get; set; }

        public TrackedOrderBuyerPage(int trackedOrderID)
        {
            this.InitializeComponent();
            TrackedOrderID = trackedOrderID;
            viewModel = new TrackedOrderViewModel(Shared.Configuration._CONNECTION_STRING_);
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
    }
}
