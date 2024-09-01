using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.DTOs
{
    public class WishlistDto
    {
        public string? UserID { get; set; }
    }
    public class ReturnWishlistDto
    {
        public int WishlistID { get; set; }
        public string? UserID { get; set; }
    }
}
