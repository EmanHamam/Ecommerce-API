using E_Commerce.Domain.DTOs;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using E_Commerce.Infrastructure.Repositories.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private ResponseDto _responseDTO;

        public ReviewController(IReviewRepository reviewRepository)
        {
            _responseDTO = new ResponseDto();
            _reviewRepository = reviewRepository;
        }
        [HttpGet("GetAllReviews")]
        public async Task<IActionResult> GetAllReviews()
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _reviewRepository.GetAllReviews();
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpGet(" GetReviewById/{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _reviewRepository.GetReviewById(id);

                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);

            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetProductReviews/{id}")]
        public async Task<IActionResult> GetProductReviews(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _reviewRepository.GetProductReviews(id);

                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);

            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetProductReviewsSummary/{id}")]
        public async Task<IActionResult> GetProductReviewsSummary(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _reviewRepository.GetProductReviewsSummary(id);

                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);

            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetUserReviews/{id}")]
        public async Task<IActionResult> GetUserReviews(string id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _reviewRepository.GetUserReviews(id);

                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);

            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetUsererReviewForProduct")]
        public async Task<IActionResult> GetUsererReviewForProduct(string userId, int prdId)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _reviewRepository.GetUsererReviewForProduct(userId,prdId);

                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);

            }
            return BadRequest(ModelState);

        }

        [HttpGet("GetTopRatedProducts")]
        public async Task<IActionResult> GetTopRatedProducts(int topN=10)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _reviewRepository.GetTopRatedProducts(topN);

                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);

            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetRecentReviewsForAllProducts")]
        public async Task<IActionResult> GetRecentReviewsForAllProducts(int count=10)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _reviewRepository.GetRecentReviewsForAllProducts(count);

                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);

            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetRecentReviewsForProduct")]
        public async Task<IActionResult> GetRecentReviewsForProduct(int prdId, int count=10)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _reviewRepository.GetRecentReviewsForProduct(prdId,count);

                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);

            }
            return BadRequest(ModelState);

        }

        [HttpPost("CreateReview")]
        public async Task<IActionResult> CreateReview(CreateReviewDto reviewDto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO=await _reviewRepository.CreateReview(reviewDto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode,_responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);

            }
            return BadRequest(ModelState);
        }
        [HttpPut("UpdateReview")]
        public async Task<IActionResult> UpdateReview(int id, CreateReviewDto reviewDto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _reviewRepository.UpdateReview(id,reviewDto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);

            }
            return BadRequest(ModelState);
        }
        [HttpDelete("DeleteReview")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _reviewRepository.DeleteReview(id);
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
