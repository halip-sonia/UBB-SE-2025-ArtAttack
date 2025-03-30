using ArtAttack.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ArtAttack
{
    public sealed partial class FinalisePurchase : Page
    {
        private FinalizePurchaseViewModel viewModel;
        public FinalisePurchase(int orderHistoryID)
        {
            this.InitializeComponent();
            viewModel = new FinalizePurchaseViewModel(orderHistoryID);
            DataContext = viewModel;
        }

        private void onContinueShopping_Clicked(object sender, RoutedEventArgs e)
        {

        }

    }
}
