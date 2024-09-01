using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageURL { get; set; }
        public int CategoryID { get; set; }
        public int BrandID { get; set; }


        public virtual Category? Category { get; set; }
        public virtual Brand? Brand { get; set; }

        [JsonIgnore]
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
        [JsonIgnore]
        public virtual ICollection<CartItem>? CartItems { get; set; }
        [JsonIgnore]
        public virtual ICollection<WishlistItem>? WishlistItems { get; set; }
        [JsonIgnore]
        public virtual ICollection<Review>? Reviews { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProductDetails>? ProductDetails { get; set; }
    }

}
