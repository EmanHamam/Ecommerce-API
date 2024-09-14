using E_Commerce.Domain.DTOs;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartItemController : ControllerBase
    {
        private readonly ICartItemRepository _cartItemCpository;
        private ResponseDto _responseDTO;

        public ShoppingCartItemController(ICartItemRepository cartItemCpository)
        {
            _cartItemCpository = cartItemCpository;
            _responseDTO = new ResponseDto();

        }
        [HttpGet("GetCartItem/{id}")]
        public async Task<IActionResult> GetCartItem(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _cartItemCpository.GetCartItem(id);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetCartItems/{id}")]
        public async Task<IActionResult> GetCartItems(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _cartItemCpository.GetItemsInCart(id);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetItemsInCartByUserId/{id}")]
        public async Task<IActionResult> GetItemsInCartByUserId(string id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _cartItemCpository.GetItemsInCartByUserId(id);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpPost("AddItemToCart")]
        public async Task<IActionResult> AddItemToCart(CartItemDto itemDto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _cartItemCpository.AddItemToCart(itemDto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpPut("UpdateCartItem")]
        public async Task<IActionResult> UpdateCartItem(int itemId, CartItemDto itemDto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _cartItemCpository.UpdateCartItem(itemId,itemDto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpDelete("RemoveCartItem")]
        public async Task<IActionResult> RemoveCartItem(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _cartItemCpository.RemoveCartItem(id);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpDelete("RemoveCartItemByProductId")]
        public async Task<IActionResult> RemoveCartItemByProductId(int cartId, int prdId)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _cartItemCpository.RemoveCartItemByProductId(cartId,prdId);
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
