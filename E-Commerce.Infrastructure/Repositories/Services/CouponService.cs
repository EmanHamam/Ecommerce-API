using AutoMapper;
using E_Commerce.Domain.DTOs;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Services
{
    public class CouponService : GenericRepository<Coupon>, ICouponService
    {
        private readonly ApplicationDbContext _context;
       
        public CouponService(ApplicationDbContext context):base(context)
        {
            _context = context;
           
        }
        public async Task<ResponseDto> ApplyCouponToOrder(Order order, string couponCode)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.CouponCode == couponCode);
            if (coupon == null || (coupon.ExpirationDate.HasValue && coupon.ExpirationDate < DateTime.Now))
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Invalid or expired coupon."
                };
            }

            var orderCoupon = new OrderCoupon
            {
                OrderID = order.OrderID,
                CouponID = coupon.CouponID
            };

            order.OrderCoupons ??= new List<OrderCoupon>();
            order.OrderCoupons.Add(orderCoupon);
            order.Subtotal -= coupon.DiscountAmount; // Apply the discount to the subtotal

            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = coupon.DiscountAmount
            };
        }

       
        public async Task<ResponseDto> RemoveCouponFromOrder(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderCoupons)
                 .FirstOrDefaultAsync(o => o.OrderID == orderId);
            if (order == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Order not found."
                };
            }

            var orderCoupons = order.OrderCoupons.ToList();
            foreach (var orderCoupon in orderCoupons)
            {
                var coupon = await _context.Coupons.FindAsync(orderCoupon.CouponID);
                if (coupon != null)
                {
                    order.Subtotal += coupon.DiscountAmount; // Revert the discount
                    _context.OrderCoupons.Remove(orderCoupon);
                }
            }

            var effectedRows = await _context.SaveChangesAsync();
            if (effectedRows > 0)
            {
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = order
                };
            }

            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "Can't remove coupon."
            };
        }
    }
}
