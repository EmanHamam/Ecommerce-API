using E_Commerce.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Interfaces
{
    public interface IOrderService
    {
        public Task<ResponseDto> GetAllOrders();
        public Task<ResponseDto> GetOrderById(int orderId);
        public Task<ResponseDto> GetOrdersByCurrentUser();
        public Task<ResponseDto> GetDeliveryMethods();

        public Task<ResponseDto> GetRecentOrders(DateTime fromDate);
        public Task<ResponseDto> GetOrdersByStatus(string status);
        public Task<ResponseDto> GetOrderSummary(int orderId);





        public Task<ResponseDto> CreateOrder(CreateOrderDto orderDto);
        public Task<ResponseDto> UpdateOrder(int orderId, CreateOrderDto orderDto);
        
        public Task<ResponseDto> UpdateOrderStatus(string paymentIntentId, string status);
      
        public Task<ResponseDto> CancelOrder(int orderId);

    }
}
