using ArtAttack.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtAttack.ViewModel
{
    public interface IBillingInfoModelView
    {
        Task<List<DummyProduct>> GetDummyProductsFromOrderHistoryAsync(int orderHistoryID);
        void CalculateOrderTotal(int orderHistoryID);
        void ApplyBorrowedTax(DummyProduct dummyProduct);

    }
}
