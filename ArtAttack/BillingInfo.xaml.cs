using ArtAttack.ViewModel;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
