using ArtAttack.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.ViewModel
{
    interface ICardInfoViewModel
    {
        Task<List<DummyProduct>> GetDummyProductsFromOrderHistoryAsync(int orderHistoryID);

        Task ProcessCardPaymentAsync();


    }
}
