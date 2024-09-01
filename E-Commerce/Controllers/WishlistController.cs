using E_Commerce.Domain.DTOs;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistRepository _wishlistRepository;
        private ResponseDto _responseDTO;

        public WishlistController(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
            _responseDTO = new ResponseDto();

        }
        [HttpGet("GetWishlistByUserId/{userId}")]
        public async Task<IActionResult> GetWishlistByUserId(string userId)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _wishlistRepository.GetWishlistByUserId(userId);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetWishlistById/{id}")]
        public async Task<IActionResult> GetWishlistById(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _wishlistRepository.GetWishlistById(id);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        
        [HttpPost("CreateWishlist")]
        public async Task<IActionResult> CreateWishlist(WishlistDto dto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _wishlistRepository.CreateWishlist(dto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpPut("UpdateWishlist")]
        public async Task<IActionResult> UpdateWishlist(int id, WishlistDto dto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _wishlistRepository.UpdateWishlist(id, dto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpDelete("ClearWishlist")]
        public async Task<IActionResult> ClearWishlist(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _wishlistRepository.ClearWishlist(id);
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
