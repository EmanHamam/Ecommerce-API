using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,

        [EnumMember(Value = "Shipped")]
        Shipped,

        [EnumMember(Value = "Delivered")]
        Delivered,

        [EnumMember(Value = "Canceled")]
        Canceled,

        [EnumMember(Value = "PaymentRecevied")]
        PaymentRecevied,
        
        [EnumMember(Value = "PaymentFailed")]
        PaymentFailed,


    }
}
