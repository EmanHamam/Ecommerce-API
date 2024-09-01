using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class CartItem
    {
        public int CartItemID { get; set; }
        public int ShoppingCartID { get; set; }
        public int ProductID { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool inTheCart { get; set; }=false;
        public virtual ShoppingCart? ShoppingCart { get; set; }
        public virtual Product? Product { get; set; }
    }

}
