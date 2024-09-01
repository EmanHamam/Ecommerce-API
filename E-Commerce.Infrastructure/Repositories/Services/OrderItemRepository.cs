using AutoMapper;
using E_Commerce.Domain.DTOs;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Services
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        

        public OrderItemRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
            
        }
        public async Task<ResponseDto> GetOrderItem(int itemId)
        {
            var item= await _context.OrderItems.Include(oi=>oi.Product).FirstOrDefaultAsync(oi=>oi.OrderItemID==itemId);
            if (item == null)
            {
                return new ResponseDto
                {
                    StatusCode=404,
                    IsSucceeded=false,
                    DisplayMessage="Order Item Not Found"
                };
            }
            var orderItemDto = _mapper.Map<OrderItemDto>(item);
            return new ResponseDto{
                StatusCode = 200,
                IsSucceeded = true,
                Result= orderItemDto
            };
        }
        public async Task<ResponseDto> GetItemsInOrder(int orderId) 
        { 
            var order= await _context.Orders.Include(o=>o.OrderItems).SingleOrDefaultAsync(o=>o.OrderID==orderId);
            if(order == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Order Not Found"
                };
            }
            var orderItems=await _context.OrderItems.Include(oi => oi.Product).Where(oi=>oi.OrderID==orderId).ToListAsync();
            var orderItemsDto = _mapper.Map<List<OrderItemDto>>(orderItems);
            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = orderItemsDto
            };

        }
        public async Task<ResponseDto> AddItemToOrder(OrderItemCreateDto itemDto)
        {
            var order = await _context.Orders.Include(o => o.OrderItems)
                                     .FirstOrDefaultAsync(o => o.OrderID == itemDto.OrderID);
            if (order==null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Order Not Found"
                };
            }
            if (!_context.Products.Any(p=>p.ProductID==itemDto.ProductID))
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Product Not Found"
                };
            }
            var orderItem = _mapper.Map<OrderItem>(itemDto);
            orderItem.Product=await _context.Products.FirstOrDefaultAsync(p=>p.ProductID==itemDto.ProductID);    
           
                orderItem.Price = orderItem.Product.Price * orderItem.Quantity;

            await _context.OrderItems.AddAsync(orderItem);
            // Update the order's subtotal
            order.Subtotal += orderItem.Price;
            order.Total += orderItem.Price;
            _context.Orders.Update(order);
            var effectedRows = await _context.SaveChangesAsync();


            if (effectedRows > 0)
            {
                return new ResponseDto
                {
                    StatusCode = 201,
                    IsSucceeded = true,
                    Result = orderItem
                };
            }

            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "Failed to add item to order."
            };

        }
        public async Task<ResponseDto> UpdateOrderItem(int itemId, OrderItemCreateDto itemDto) 
        {
            var item = await _context.OrderItems.FindAsync(itemId);
            if (item == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Order Item Not Found"
                };
            }


            var order = await _context.Orders.Include(o => o.OrderItems)
                                     .FirstOrDefaultAsync(o => o.OrderID == itemDto.OrderID);
            if (order == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Order Not Found"
                };
            }
            
            if (!_context.Products.Any(p => p.ProductID == itemDto.ProductID))
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Product Not Found"
                };
            }
            _mapper.Map(itemDto, item);

            item.Product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == itemDto.ProductID);

            item.Price = item.Product.Price * item.Quantity;


            _context.OrderItems.Update(item);
            order.Subtotal += item.Price;
            order.Total += item.Price;
            _context.Orders.Update(order);
            var effectedRows = await _context.SaveChangesAsync();

            if (effectedRows > 0)
            {
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = item
                };
            }

            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "Failed to update order item."
            };

        }
        public async Task<ResponseDto> RemoveOrderItem(int itemId)
        {
            var orderItem = await _context.OrderItems.FindAsync(itemId);
            if (orderItem == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Order item not found."
                };
            }
            var order = await _context.Orders.Include(o => o.OrderItems)
                         .FirstOrDefaultAsync(o => o.OrderID == orderItem.OrderID);
            order.Subtotal -= orderItem.Price;
            order.Total -= orderItem.Price;
            _context.Orders.Update(order);
            _context.OrderItems.Remove(orderItem);
            var effectedRows = await _context.SaveChangesAsync();

            return new ResponseDto
            {
                StatusCode = effectedRows > 0 ? 200 : 400,
                IsSucceeded = effectedRows > 0,
                DisplayMessage = effectedRows > 0 ? "Order item removed successfully." : "Failed to remove order item."
            };

           

        }
    }
}
