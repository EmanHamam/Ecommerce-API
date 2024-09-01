using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public string? UserID { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public string? PaymentMethod { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser? AppUser { get; set; }
    }
}
