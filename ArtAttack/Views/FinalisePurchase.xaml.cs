using ArtAttack.ViewModel;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
