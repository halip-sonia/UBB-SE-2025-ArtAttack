using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.Domain
{
    class TrackedOrder
    {
        public int TrackedOrderID { get; set; }
        public int OrderID { get; set; }
        public OrderStatus CurrentStatus { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public string DeliveryAddress { get; set; }

    }
}
