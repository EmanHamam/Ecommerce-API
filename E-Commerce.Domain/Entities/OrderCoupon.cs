using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace E_Commerce.Domain.Entities
{
    public class OrderCoupon
    {
        public int OrderCouponID { get; set; }
        public int OrderID { get; set; }
        public int CouponID { get; set; }

        public virtual Order?Order { get; set; }
        public virtual Coupon? Coupon { get; set; }
    }
}
