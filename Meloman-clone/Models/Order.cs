using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Meloman_clone.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public string Telephone { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string DeliveryType { get; set; }
        public string Address { get; set; }
        public string IsExpressDelivery { get; set; }
        public string PaymentType { get; set; }
        public int InitialPrice { get; set; }
        public decimal Discount { get; set; }
        public int FinalPrice { get; set; }
        public string Comments { get; set; }
        public string Date { get; set; }
        [ForeignKey("OrderProducts")]
        public List<OrderProduct> Products { get; set; } = new List<OrderProduct>();
        public string DeliveryStatus { get; set; }
    }
}
