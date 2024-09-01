using E_Commerce.Domain.DTOs;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using E_Commerce.Infrastructure.Repositories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private ResponseDto _responseDTO;

        public OrderController(IOrderService orderService)
        {
            _responseDTO = new ResponseDto();
            _orderService = orderService;
        }
        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _orderService.GetAllOrders();
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetOrderById/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _orderService.GetOrderById(id);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetOrdersByCurrentUser")]
        public async Task<IActionResult> GetOrdersByCurrentUser()
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _orderService.GetOrdersByCurrentUser();
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetRecentOrders")]
        public async Task<IActionResult> GetRecentOrders(DateTime fromDate)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _orderService.GetRecentOrders(fromDate);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetOrdersByStatus/{status}")]
        public async Task<IActionResult> GetOrdersByStatus(string status)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _orderService.GetOrdersByStatus( status);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetOrderSummary/{id}")]
        public async Task<IActionResult> GetOrderSummary(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _orderService.GetOrderSummary(id);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }





        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder(CreateOrderDto orderDto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _orderService.CreateOrder(orderDto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpPut("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(int orderId, CreateOrderDto orderDto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _orderService.UpdateOrder(orderId,orderDto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpPut("UpdateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatus(string paymentIntentId, string status)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _orderService.UpdateOrderStatus(paymentIntentId,  status);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpPut(" CancelOrder")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _orderService.CancelOrder(orderId);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }

    }
}
