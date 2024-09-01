using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class Order
    {
        public int OrderID { get; set; }
        public string? UserID { get; set; }
        public DateTime ShippingDate { get; set; } = DateTime.Now;
        public decimal Subtotal { get; set; }//cal
        public decimal? Total { get; set; }//cal
        public int? DeliveryMethodID {  get; set; }
        [ForeignKey(nameof(DeliveryMethodID))]
        public DeliveryMethod? DeliveryMethod { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public string PaymentIntentId { get; set; }
        public OrderShippingAddress ShippingAddress { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser? AppUser { get; set; }

        [JsonIgnore]
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
        [JsonIgnore]
        public virtual ICollection<OrderCoupon>? OrderCoupons { get; set; }
       

        
    }
}
