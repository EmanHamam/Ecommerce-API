using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class ShoppingCart
    {
        public int ShoppingCartID { get; set; }
        public string? UserID { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int? DeliveryMethodId { get; set; }
        public string? ClientSecret { get; set; } 
        public string? PaymentIntentId { get; set; } 
        public decimal? ShippingPrice { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser? AppUser { get; set; }
        [JsonIgnore]
        public virtual ICollection<CartItem>? CartItems { get; set; }
    }
}
