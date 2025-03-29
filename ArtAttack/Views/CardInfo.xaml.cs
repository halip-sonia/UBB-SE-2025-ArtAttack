using ArtAttack.Domain;
using ArtAttack.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack
{
    public sealed partial class CardInfo : Page
    {
        private CardInfoViewModel viewModel;

        public CardInfo(int orderHistoryID)
        {
            this.InitializeComponent();
            viewModel = new CardInfoViewModel(orderHistoryID);
            DataContext = viewModel;
        }
        private async void onPayButtonClickedAsync(object sender, RoutedEventArgs e)
        {
            if (DataContext is CardInfoViewModel viewModel)
            {
                await viewModel.onPayButtonClickedAsync();

            }
        }

    }
}
