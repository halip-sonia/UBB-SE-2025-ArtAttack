using ArtAttack.Domain;
using ArtAttack.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ArtAttack
{
    public sealed partial class BillingInfo : Page
    {
        private BillingInfoModelView viewModel;

        public BillingInfo(int orderHistoryID)
        {
            this.InitializeComponent();
            viewModel = new BillingInfoModelView(orderHistoryID);

            DataContext = viewModel;
        }
        private async void onFinalizeButtonClickedAsync(object sender, RoutedEventArgs e)
        {
            if (DataContext is BillingInfoModelView viewModel)
            {
                await viewModel.onFinalizeButtonClickedAsync();

            }
        }

        private void OnStartDateChanged(DatePicker sender, DatePickerSelectedValueChangedEventArgs e)
        {
            UpdateBorrowedProductTax(sender);
        }

        private void OnEndDateChanged(DatePicker sender, DatePickerSelectedValueChangedEventArgs e)
        {
            UpdateBorrowedProductTax(sender);
        }

        private void UpdateBorrowedProductTax(DatePicker sender)
        {
            if (DataContext is BillingInfoModelView viewModel && sender.DataContext is DummyProduct product)
            {
                viewModel.ApplyBorrowedTax(product);
            }
        }
    }
}
