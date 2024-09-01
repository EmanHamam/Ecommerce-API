using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class Brand
    {
        public int BrandID { get; set; }
        public string BrandName { get; set; }

        [JsonIgnore]
        public virtual ICollection<Product>? Products { get; set; }
    }
}
