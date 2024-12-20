﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.DTOs
{
    public class WishlistItemDto
    {
        public int WishlistID { get; set; }
        public int ProductID { get; set; }
        public bool isFav { get; set; } = false;
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
    public class ReturnWishlistItemDto
    {
        public int WishlistID { get; set; }
        public int WishlistItemID { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? ProductImage { get; set; }
        public decimal? Price { get; set; }
        public bool isFav { get; set; } = false;
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}
