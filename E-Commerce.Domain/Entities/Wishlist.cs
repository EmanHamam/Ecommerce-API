using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class Wishlist
    {
        public int WishlistID { get; set; }
        public string? UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser? AppUser { get; set; }
        [JsonIgnore]
        public virtual ICollection<WishlistItem>? WishlistItems { get; set; }
    }
}
