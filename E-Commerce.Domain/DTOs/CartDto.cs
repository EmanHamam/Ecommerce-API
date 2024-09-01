using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.DTOs
{
    public class CartDto
    {
        //public string? UserID { get; set; }
        public int ShoppingCartID { get; set; }
        public List<ReturnCartItemDto>? Items { get; set; } = [];
        //public DateTime? LastUpdated { get; set; }= DateTime.UtcNow;
        public int? DeliveryMethodId { get; set; }
        public string ClientSecret { get; set; } = string.Empty;
        public string PaymentIntentId { get; set; } = string.Empty;
        public decimal ShippingPrice { get; set; } = 0;


    }
    public class UpdateCartDto
    {
       // public int ShoppingCartID { get; set; }
        public List<CartItemDto>? Items { get; set; }
        public int? DeliveryMethodId { get; set; }
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public decimal ShippingPrice { get; set; }
    }
    public class ReturnCartDto
    {
        public int ShoppingCartID { get; set; }
        public string? UserID { get; set; }
       // public DateTime? LastUpdated { get; set; } = DateTime.UtcNow;
        public List<ReturnCartItemDto>? CartItems { get; set; }
        //public int? DeliveryMethodId { get; set; }
        //public string ClientSecret { get; set; }
        //public string PaymentIntentId { get; set; }
        //public decimal ShippingPrice { get; set; }


    }
}
