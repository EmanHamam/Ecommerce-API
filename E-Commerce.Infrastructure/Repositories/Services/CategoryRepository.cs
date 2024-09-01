using AutoMapper;
using E_Commerce.Domain.DTOs;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Services
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<ResponseDto> GetAllCategories()
        {
            var Categories = await _context.Categories
                .AsNoTracking()
                .ToListAsync();
            if (Categories == null)
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = "there is no Categories"
                };
            }

            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = Categories,

            };
        }
        public async Task<ResponseDto> GetCategoryById(int id)
        {
            var Category = await _context.Categories
                .Where(c => c.CategoryID == id)
               .FirstOrDefaultAsync();
            if (Category == null)
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = $"Category with ID {id} doesn't exist"
                };
            }
            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = Category
            };

        }
        public async Task<ResponseDto> GetCategoryByName(string name)
        {
            var Category = await _context.Categories
                .Where(c => c.CategoryName == name)
                .FirstOrDefaultAsync();

            if (Category == null)
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = "no Category Was found for this Name"
                };
            }

            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = Category,

            };


        }

        public async Task<ResponseDto> AddCategory(CategoryDTO dto)
        {
            var CategoryExists = await _context.Categories.AnyAsync(c => c.CategoryName == dto.CategoryName);
            if (!CategoryExists)
            {
                var Category = new Category
                {
                    CategoryName = dto.CategoryName,
                };


                await _context.Categories.AddAsync(Category);
                await _context.SaveChangesAsync();

                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = Category
                };

            }

            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "this Category already Exist!"
            };

        }

        public async Task<ResponseDto> UpdateCategory(int id, CategoryDTO dto)
        {
            var Category = await _context.Categories.FindAsync(id);
            if (Category == null)
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = $"Category with ID {id} Not Found"
                };
            }

            Category.CategoryName = dto.CategoryName;

            await _context.SaveChangesAsync();
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = Category,
                };
        }
   
        public async Task<ResponseDto> DeleteCategory(int id)
        {
        var Category = await _context.Categories.FindAsync(id);
        if (Category == null)
        {
            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = $"Category with ID {id} Not Found"
            };
        }
            _context.Categories.Remove(Category);
            await _context.SaveChangesAsync();
            return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = Category,
                };
        }

        

        
    }
}
