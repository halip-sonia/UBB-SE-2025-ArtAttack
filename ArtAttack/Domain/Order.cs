using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.Domain
{
    internal class Order
    {
        public int OrderID { get; set; }

        public int ProductID { get; set; }
        public int BuyerID { get; set; }
        public int ProductType { get; set; }  
        public int OrderSummaryID { get; set; }

        public int OrderHistoryID { get; set; }

        public string PaymentMethod { get; set; }
        public DateTime OrderDate { get; set; }

        public string FormattedDate => OrderDate.ToString("yyyy-MM-dd");

        public virtual DummyProduct Product { get; set; }
        public virtual DummyBuyer Buyer { get; set; }
        public virtual OrderSummary OrderSummary { get; set; }
    }
}
