using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.DTOs
{
    public class CategoryDTO
    {
        [Required]
        public string CategoryName {  get; set; }
    }
}
