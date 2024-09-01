using E_Commerce.Domain.DTOs;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistItemController : ControllerBase
    {
        private readonly IWishlistItemRepository _wishlistItemRepository;
        private ResponseDto _responseDTO;

        public WishlistItemController(IWishlistItemRepository wishlistItemRepository)
        {
            _wishlistItemRepository = wishlistItemRepository;
            _responseDTO = new ResponseDto();

        }
        [HttpGet("GetWishlistItem/{id}")]
        public async Task<IActionResult> GetWishlistItem(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _wishlistItemRepository.GetWishlistItem(id);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetWishlistItems/{id}")]
        public async Task<IActionResult> GetWishlistItems(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _wishlistItemRepository.GetWishlistItems(id);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpPost("AddItemToWishlist")]
        public async Task<IActionResult> AddItemToWishlist(WishlistItemDto itemDto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _wishlistItemRepository.AddItemToWishlist(itemDto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpPut("UpdateWishlistItem")]
        public async Task<IActionResult> UpdateWishlistItem(int itemId, WishlistItemDto itemDto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _wishlistItemRepository.UpdateWishlistItem(itemId, itemDto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpDelete("RemoveWishlistItem")]
        public async Task<IActionResult> RemoveWishlistItem(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _wishlistItemRepository.RemoveWishlistItem(id);
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
