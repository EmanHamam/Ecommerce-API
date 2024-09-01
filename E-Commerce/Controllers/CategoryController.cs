using E_Commerce.Domain.DTOs;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using E_Commerce.Infrastructure.Repositories.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private ResponseDto _responseDTO;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _responseDTO = new ResponseDto();
            _categoryRepository = categoryRepository;
        }
        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _categoryRepository.GetAllCategories();
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _categoryRepository.GetCategoryById(id);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }


        [HttpGet("GetCategoryByName/{name}")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _categoryRepository.GetCategoryByName(name);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }


        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromForm]CategoryDTO dto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _categoryRepository.AddCategory(dto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(int id,[FromForm] CategoryDTO dto)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _categoryRepository.UpdateCategory(id,dto);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);

        }
        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (ModelState.IsValid)
            {
                _responseDTO = await _categoryRepository.DeleteCategory(id);
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
