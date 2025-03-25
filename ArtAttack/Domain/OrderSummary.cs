using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.Domain
{
    internal class OrderSummary
    {
        public int ID { get; set; }  

        public double Subtotal { get; set; }
        public double WarrantyTax { get; set; }
        public double DeliveryFee { get; set; }
        public double FinalTotal { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string AdditionalInfo { get; set; }
        public string ContractDetails { get; set; }  

        public virtual ICollection<Order> Orders { get; set; }
    }
}
