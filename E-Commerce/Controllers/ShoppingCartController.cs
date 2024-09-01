using E_Commerce.Domain.DTOs;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private ResponseDto _responseDTO;

        public ShoppingCartController(ICartRepository cartRepository)
        {
            _cartRepository=cartRepository;
            _responseDTO = new ResponseDto();

        }
        [HttpGet("GetCartByUser")]
        public async Task<IActionResult> GetCartByUser()
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _cartRepository.GetCartByUser();
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetCartById/{id}")]
        public async Task<IActionResult> GetCartById(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _cartRepository.GetCartById(id);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetPopularSalledProducts")]
        public async Task<IActionResult> GetPopularSalledProducts()
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _cartRepository.GetPopularSalledProducts();
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpPost("CreateCart")]
        public async Task<IActionResult> CreateCart()
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _cartRepository.CreateCart();
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpPut("UpdateCart/{id}")]
        public async Task<IActionResult> UpdateCart(int id, [FromBody] UpdateCartDto dto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _cartRepository.UpdateCart(id,dto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpDelete("ClearCart")]
        public async Task<IActionResult> ClearCart(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _cartRepository.ClearCart(id);
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
