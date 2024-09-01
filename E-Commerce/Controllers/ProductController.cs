using AutoMapper;
using E_Commerce.Domain.DTOs;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using E_Commerce.Infrastructure.Repositories.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private ResponseDto _responseDTO;

        public ProductController(IProductRepository productRepository)
        {
            _responseDTO = new ResponseDto();
            _productRepository = productRepository;
        }
        

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductParams? productParams)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _productRepository.GetAllProducts(productParams);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }

            return BadRequest(ModelState);

        }
        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _productRepository.GetProductById(id);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetProductDetailsById/{id}")]
        public async Task<IActionResult> GetProductDetailsById(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _productRepository.GetProductDetailsById(id);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
       
        [HttpGet("GetProductsByCategoryId/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategoryId(int categoryId, [FromQuery] ProductParams? productParams)
        {     
            if (ModelState.IsValid)
            {
                _responseDTO = await _productRepository.GetProductsByCategoryId(categoryId, productParams);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetProductsByBrand/{brand}")]
        public async Task<IActionResult> GetProductsByBrand(string brand)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _productRepository.GetProductsByBrand(brand);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetAllProductIDs")]
        public async Task<ActionResult<IEnumerable<int>>> GetAllProductIDs()
        {
            var ids = await _productRepository.GetAllIDs();

         
            if (ids == null || !ids.Any())
            {
                return NotFound("No product IDs found.");
            }

            return Ok(ids);
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm]CreateProductDto dto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _productRepository.AddProduct(dto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpPost("AddProductDetails")]
        public async Task<IActionResult> AddProductDetails([FromForm]CreateProductDetailsDto dto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _productRepository.AddProductDetails(dto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }

        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id,[FromForm] UpdateProductDto dto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _productRepository.UpdateProduct(id,dto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpPut("UpdateProductDetauls/{id}")]
        public async Task<IActionResult> UpdateProductDetails(int id, CreateProductDetailsDto dto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _productRepository.UpdateProductDetails( id,  dto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _productRepository.DeleteProduct(id);
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
