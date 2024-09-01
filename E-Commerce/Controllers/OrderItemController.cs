using E_Commerce.Domain.DTOs;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using E_Commerce.Infrastructure.Repositories.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private ResponseDto _responseDTO;
        public OrderItemController(IOrderItemRepository orderItemRepository)
        {
            _responseDTO = new ResponseDto();
            _orderItemRepository = orderItemRepository;
        }

        [HttpGet("GetOrderItem")]
        public async Task<IActionResult> GetOrderItem(int itemId)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _orderItemRepository.GetOrderItem(itemId);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetItemsInOrder")]
        public async Task<IActionResult> GetItemsInOrder(int orderId)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _orderItemRepository.GetItemsInOrder(orderId);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }

        [HttpPost("AddItemToOrder")]
        public async Task<IActionResult> AddItemToOrder(OrderItemCreateDto itemDto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _orderItemRepository.AddItemToOrder(itemDto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpPut("UpdateOrderItem")]
        public async Task<IActionResult> UpdateOrderItem(int itemId, OrderItemCreateDto itemDto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _orderItemRepository.UpdateOrderItem(itemId, itemDto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpDelete("RemoveOrderItem")]
        public async Task<IActionResult> RemoveOrderItem(int itemId)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _orderItemRepository.RemoveOrderItem(itemId);
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
