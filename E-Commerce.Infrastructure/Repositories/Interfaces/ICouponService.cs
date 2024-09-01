using E_Commerce.Domain.DTOs;
using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Interfaces
{
    public interface ICouponService
    {
        public Task<ResponseDto> ApplyCouponToOrder(Order order, string couponCode);
        public Task<ResponseDto> RemoveCouponFromOrder(int orderId);
    }
}
