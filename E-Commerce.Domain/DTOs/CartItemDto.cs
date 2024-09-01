using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.DTOs
{
    public class CartItemDto
    {

        public int ShoppingCartID { get; set; }
        public int ProductID { get; set; }
        public decimal Price { get; set; }

        public int Quantity { get; set; }
        public bool inTheCart { get; set; } = false;
    }
    public class ReturnCartItemDto
    {
        //public int CartItemID { get; set; }
        //public int ShoppingCartID { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; } 
        public string? ProductImage { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public int StockQuantity {  get; set; }
        public bool inTheCart { get; set; } = false;
    }
}
