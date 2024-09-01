using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public class Coupon
    {
        public int CouponID { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime? ExpirationDate { get; set; }

        //public decimal? MinimumOrderValue { get; set; }
        [JsonIgnore]
        public virtual ICollection<OrderCoupon>? OrderCoupons { get; set; }
    }
}
