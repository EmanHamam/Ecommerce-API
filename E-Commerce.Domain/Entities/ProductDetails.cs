using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class ProductDetails
    {
        public int ProductDetailsID { get; set; }
        public int ProducID { get; set; }
        public string SmallImg1 { get; set; }
        public string SmallImg2 { get; set; }
        public string SmallImg3 { get; set; }

        [ForeignKey(nameof(ProducID))]
        public virtual Product? Product { get; set; }



    }
}
