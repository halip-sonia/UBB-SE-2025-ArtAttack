using ArtAttack.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.ViewModel
{
    public interface IBillingInfoModelView
    {
        public List<DummyProduct> GetDummyProductsFromOrderHistory(int orderHistoryID);
        public void CalculateOrderTotal(int orderHistoryID);
        public void ApplyBorrowedTax(int productID, DateTime startDate, DateTime endDate);
        //public void ProcessCardPayment(int orderHistoryID);


    }
}
