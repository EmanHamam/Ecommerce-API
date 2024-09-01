using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.DTOs
{
    public class OrderDto
    {
        public int OrderID { get; set; }
        public DateTime? ShippingDate { get; set; } = DateTime.Now;
        public DateTime? DeliveringDate { get; set; }
        public string? DeliveryMethodName { get; set; }
        public string? OrderStatus { get; set; }
        public Address? ShippingAddress { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? DeliveryPrice { get; set; }
       // public decimal? CouponDisCount { get; set; }
        public decimal? Total {  get; set; }
        public List<OrderItemDto> OrderItems { get; set; }

    }
    public class CreateOrderDto
    {
        //public string? UserID { get; set; }
        public int? CartId { get; set; }
        public OrderShippingAddressDto? ShippingAddress { get; set; }

        public int? DeliveryMethodID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ShippingDate { get; set; } = DateTime.Now;

        public OrderStatus? OrderStatus { get; set; }
        public string PaymentIntentId { get; set; }


        public string? CouponCode { get; set; }

    }

}
