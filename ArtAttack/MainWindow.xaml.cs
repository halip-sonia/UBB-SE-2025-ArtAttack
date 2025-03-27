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
using ArtAttack.ViewModel;
using ArtAttack.Shared;

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
            //example contract
            contract = _contractViewModel.GetContractById(1);
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

        private void generateContractButton_Click(object sender, RoutedEventArgs e)
        {
            if (contract != null)
            {
                _contractViewModel.GenerateAndSaveContract(contract);
                //show a message to the user indicating the PDF was saved.
            }
            else
            {
                // Handle the error appropriately if the contract is null.
            }
        }
    }
}
