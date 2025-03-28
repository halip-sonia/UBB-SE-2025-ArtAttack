﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.Domain
{
    internal class TrackedOrder
    {
        public int TrackedOrderID { get; set; }
        public int OrderID { get; set; }
        public OrderStatus CurrentStatus { get; set; }
        public DateOnly EstimatedDeliveryDate { get; set; }
        public required string DeliveryAddress { get; set; }
    }
}
