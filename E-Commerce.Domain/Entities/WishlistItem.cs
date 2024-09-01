using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class WishlistItem
    {
        public int WishlistItemID { get; set; }
        public int WishlistID { get; set; }
        public int ProductID { get; set; }
        public bool isFav { get; set; }=false;
        public DateTime DateAdded { get; set; }=DateTime.Now;

        public virtual Wishlist? Wishlist { get; set; }
        public virtual Product? Product { get; set; }
    }
}
