using E_Commerce.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        public Task<ResponseDto> GetOrderItem(int itemId);
        public Task<ResponseDto> GetItemsInOrder(int orderId);
        public Task<ResponseDto> AddItemToOrder(OrderItemCreateDto itemDto);
        public Task<ResponseDto> UpdateOrderItem(int itemId, OrderItemCreateDto itemDto);
        public Task<ResponseDto> RemoveOrderItem(int itemId);
    }
}
