using AutoMapper;
using E_Commerce.Domain.DTOs;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order = E_Commerce.Domain.Entities.Order;

namespace E_Commerce.Infrastructure.Repositories.Services
{
    public class OrderService:GenericRepository<Order>, IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICouponService _couponService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(ApplicationDbContext context, IMapper mapper,
            UserManager<ApplicationUser> userManager, ICouponService couponService, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _couponService = couponService;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<ResponseDto> GetAllOrders() 
        { 
            var orders= await _context.Orders.Include(o=>o.OrderItems).Include(o=>o.DeliveryMethod).AsNoTracking().ToListAsync();

            if (orders == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "No Orders Found"
                };
            }
            var orderDtos = _mapper.Map<List<OrderDto>>(orders);

            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = orderDtos
            };
        }
        public async Task<ResponseDto> GetOrderById(int orderId) 
        {
            var order=await _context.Orders.Include(o => o.DeliveryMethod).Include(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o=>o.OrderID==orderId);
            if (order == null)
            {
                return new ResponseDto
                {
                    StatusCode=404,
                    IsSucceeded=false,
                    DisplayMessage="Order Not Found"
                };
            }
            var orderDto=_mapper.Map<OrderDto>(order);
            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = orderDto
            };

        }

        public async Task<ResponseDto> GetOrdersByCurrentUser() 
        {
            string userId = await GetCurrentUserAsync();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "User not found."
                };
            }

            var orders = await _context.Orders.Where(o => o.UserID==user.Id).Include(o => o.OrderItems).ToListAsync();

            if (!orders.Any())
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = $"There is no Orders Found for This User."
                };
            }
            var orderDtos = _mapper.Map<List<OrderDto>>(orders);
            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = orderDtos
            };
        }
        public async Task<ResponseDto> GetDeliveryMethods()
        {
            var deliveryMethods = await _context.DeliveryMethod.AsNoTracking().ToListAsync();

            if (deliveryMethods == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "No deliveryMethods Found"
                };
            }
         

            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = deliveryMethods
            };
        }
        public async Task<ResponseDto> GetRecentOrders(DateTime fromDate)
        {
            var orders=await _context.Orders.Where(o=>o.ShippingDate>= fromDate).Include(o=>o.OrderItems).ThenInclude(oi=>oi.Product).ToListAsync();
            if(!orders.Any())
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = $"There is no Orders Found From this Date {fromDate}."
                };
            }
            var orderDtos = _mapper.Map<List<OrderDto>>(orders);
            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = orderDtos
            };

        }
        public async Task<ResponseDto> GetOrdersByStatus(string status) 
        {
            var orderStatus=(OrderStatus) Enum.Parse(typeof(OrderStatus),status,true);
            var orders= await _context.Orders.Where(o=>o.OrderStatus==orderStatus).Include(o=>o.OrderItems).ToListAsync();
            if(orders!=null&&orders.Count>0)
            {
                var ordersDto = _mapper.Map<List<OrderDto>>(orders);
                return new ResponseDto
                {
                    StatusCode=200,
                    IsSucceeded=true,
                    Result=ordersDto
                };

            }
            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage=$"There is no Orders For {status} Status."
            };


        }
        public async Task<ResponseDto> GetOrderSummary(int orderId)
        {
            var order = await _context.Orders.Include(o => o.OrderItems)
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
            var orderSummary = new
            {
                order.OrderID,
                order.ShippingDate,
                order.Subtotal,
                Total = order.Total,
                order.OrderStatus,
                Items = order.OrderItems.Select(item => new
                {
                    item.ProductID,
                    item.Quantity,
                    item.Price
                })
            };

            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = orderSummary
            };


        }


        public async Task<ResponseDto> CreateOrder(CreateOrderDto orderDto)
        {
            var cart=await _context.ShoppingCarts
                .Include(c=>c.CartItems)
                .ThenInclude(ci=>ci.Product)
                 .FirstOrDefaultAsync(c=>c.ShoppingCartID==orderDto.CartId);

            if (cart == null||!cart.CartItems.Any())
            {
                return new ResponseDto
                {
                    StatusCode=404,
                    IsSucceeded = false,
                    DisplayMessage="Cart not found or empty"
                };

            }

            var order = _mapper.Map<Order>(orderDto);
            order.UserID = await GetCurrentUserAsync();
            //order.ShippingAddressID = orderDto.ShippingAddressID ?? 0;
            order.ShippingAddress = _mapper.Map<OrderShippingAddress>(orderDto.ShippingAddress);
            order.OrderStatus = orderDto.OrderStatus ?? OrderStatus.Pending;
            order.DeliveryMethod = await _context.DeliveryMethod
                   .FirstOrDefaultAsync(d => d.DeliveryMethodId == orderDto.DeliveryMethodID);
            order.OrderItems = cart.CartItems.Select(item => new OrderItem
            {
                ProductID = item.ProductID,
                Quantity = item.Quantity,
                Price = item.Price * item.Quantity,
            }).ToList();

            // Calculate the subtotal
            order.Subtotal = order.OrderItems.Sum(i => i.Price);


            // Apply coupon if provided
            if (!string.IsNullOrEmpty(orderDto.CouponCode))
            {
                var couponResponse = await _couponService.ApplyCouponToOrder(order, orderDto.CouponCode);

                if (!couponResponse.IsSucceeded)
                {
                    return couponResponse;
                }
                
            }
            order.Total = order.Subtotal + (order.DeliveryMethod?.Price ?? 0m);
            await _context.Orders.AddAsync(order);
            var effectedRows = await _context.SaveChangesAsync();

            if (effectedRows > 0)
            {
                return new ResponseDto
                {
                    StatusCode = 201,
                    IsSucceeded = true,
                    Result = order
                };
            }

            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "Unable to create order."
            };
        }

        public async Task<ResponseDto> UpdateOrder(int orderId, CreateOrderDto orderDto)
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
            _mapper.Map(orderDto, order);
            order.DeliveryMethod = _context.DeliveryMethod.FirstOrDefault(d => d.DeliveryMethodId == orderDto.DeliveryMethodID);

            // Apply coupon if provided
            if (!string.IsNullOrEmpty(orderDto.CouponCode))
            {
                 await _couponService.RemoveCouponFromOrder(orderId);

                var couponResponse = await _couponService.ApplyCouponToOrder(order, orderDto.CouponCode);
                if (!couponResponse.IsSucceeded)
                {
                    return couponResponse;
                }
            }
            else
            {
                // Remove any existing coupons if no new coupon is provided
                var removeCouponResponse = await _couponService.RemoveCouponFromOrder(orderId);
                if (!removeCouponResponse.IsSucceeded)
                {
                    return removeCouponResponse;
                }
            }
           
            if(order.DeliveryMethod!=null)
            order.Total = order.Subtotal + order.DeliveryMethod!.Price;

            else order.Total = order.Subtotal;

            _context.Orders.Update(order);
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
                DisplayMessage = "Unable to update order."
            };
        }


        public async Task<ResponseDto> UpdateOrderStatus(string paymentIntentId, string status)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.PaymentIntentId == paymentIntentId);
            if (order == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Order not found."
                };
            }

            if (!Enum.TryParse<OrderStatus>(status, out var orderStatus))
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = "Invalid order status."
                };
            }

            order.OrderStatus = orderStatus;
            _context.Orders.Update(order);

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
                DisplayMessage = "Failed to update order status."
            };
        }
        public async Task<ResponseDto> CancelOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if(order == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Order nt Found ."
                };
            }
            order.OrderStatus = OrderStatus.Canceled;
            _context.Orders.Update(order);

            var effectedRows = await _context.SaveChangesAsync();

            return new ResponseDto
            {
                StatusCode = effectedRows > 0 ? 200 : 400,
                IsSucceeded = effectedRows > 0,
                DisplayMessage = effectedRows > 0 ? "Order canceled successfully." : "Failed to cancel order."
            };
        }
        private async Task<string> GetCurrentUserAsync()
        {
            var userClaim = _httpContextAccessor.HttpContext!.User;

            // Get the current user from claims
            var user = await _userManager.GetUserAsync(userClaim);

            return user.Id;
        }

    }
}
