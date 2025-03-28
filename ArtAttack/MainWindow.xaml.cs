using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ArtAttack.Domain;
using ArtAttack.Services;
using ArtAttack.ViewModel;
using ArtAttack.Shared;
using Windows.UI.Popups;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ArtAttack
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {

        private Contract contract;
        private IContractViewModel _contractViewModel;

        public MainWindow()
        {
            this.InitializeComponent();
            contract = new Contract();
            _contractViewModel = new ContractViewModel(Configuration._CONNECTION_STRING_);
        }

        // This event handler is called when the Grid (root element) is loaded.
        private async void RootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            // Asynchronously fetch the contract after the UI is ready.
            contract = await _contractViewModel.GetContractByIdAsync(1);
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Now you await the async method.
            contract = await _contractViewModel.GetContractByIdAsync(1);
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
        }

        private void purchaseButton_Click(object sender, RoutedEventArgs e)
        {
            BillingInfoWindow billingInfoWindow = new BillingInfoWindow();
            var bp = new BillingInfo(1);
            billingInfoWindow.Content = bp;
            billingInfoWindow.Activate();
        }

        private void bidProductButton_Clicked(object sender, RoutedEventArgs e)
        {
            BillingInfoWindow billingInfoWindow = new BillingInfoWindow();
            var bp = new BillingInfo(2);
            billingInfoWindow.Content = bp;
            billingInfoWindow.Activate();
        }

        
        private void walletrefillButton_Clicked(object sender, RoutedEventArgs e)
        {
            BillingInfoWindow billingInfoWindow = new BillingInfoWindow();
            var bp = new BillingInfo(3);
            billingInfoWindow.Content = bp;
            billingInfoWindow.Activate();
        }

        private async void generateContractButton_Click(object sender, RoutedEventArgs e)
        {
            if (contract != null)
            {
                await _contractViewModel.GenerateAndSaveContractAsync(contract, PredefinedContractType.Borrowing);

                // Optionally, show a success dialog after generating the contract.
                var successDialog = new ContentDialog
                {
                    Title = "Success",
                    Content = "Contract generated and saved successfully.",
                    CloseButtonText = "OK",
                    XamlRoot = RootGrid.XamlRoot
                };
                await successDialog.ShowAsync();
            }
            else
            {
                await ShowNoContractDialogAsync();
            }
        }

        private async Task ShowNoContractDialogAsync()
        {
            var contentDialog = new ContentDialog
            {
                Title = "Error",
                Content = "No Contract has been found with ID 1.",
                CloseButtonText = "OK",
                XamlRoot = RootGrid.XamlRoot
            };

            await contentDialog.ShowAsync();
        }

        private async void borrowButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Example product ID - replace with actual value from your system
                int productId = 6; // Watercolor Paint Set (available product)

                var borrowWindow = new BorrowProductWindow(Configuration._CONNECTION_STRING_, productId);
                borrowWindow.Activate();
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync("Failed to open Borrow Product", ex.Message);
            }
        }

        private async Task ShowErrorDialogAsync(string title, string message)
        {
            // Create a temporary ContentDialog without relying on RootGrid
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK"
            };

            // Apply default WinUI styling
            dialog.XamlRoot = this.Content.XamlRoot;
            await dialog.ShowAsync();
        }
    }
}
