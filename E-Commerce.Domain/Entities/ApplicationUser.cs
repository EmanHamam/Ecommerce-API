 using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Address Address { get; set; }
       
        [JsonIgnore]
        public List<RefreshToken>? RefreshTokens { get; set; }
        [JsonIgnore]
        public virtual ShoppingCart? ShoppingCart { get; set; }
        [JsonIgnore]
        public virtual ICollection<Order>? Orders { get; set; }
        [JsonIgnore]
        public virtual ICollection<Review>? Reviews { get; set; }
        [JsonIgnore]
        public virtual ICollection<Wishlist>? Wishlists { get; set; }

        [JsonIgnore]
        public virtual ICollection<Payment>? Payments { get; set; }


    }
}
