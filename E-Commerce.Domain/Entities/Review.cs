using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class Review
    {
        public int ReviewID { get; set; }
        public string? AppUserID { get; set; }
        public int? ProductID { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime? ReviewDate { get; set; }= DateTime.Now;

        [ForeignKey("AppUserID")]
        public virtual ApplicationUser? AppUser { get; set; }
        public virtual Product? Product { get; set; }
    }

}
