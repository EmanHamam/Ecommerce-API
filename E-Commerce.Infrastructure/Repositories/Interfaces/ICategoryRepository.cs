using E_Commerce.Domain.DTOs;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        public  Task<ResponseDto> GetAllCategories();


        public Task<ResponseDto> GetCategoryById(int id);


        public Task<ResponseDto> GetCategoryByName(string name);

        public  Task<ResponseDto> AddCategory(CategoryDTO dto);

        public Task<ResponseDto> UpdateCategory(int id, CategoryDTO dto);

        public Task<ResponseDto> DeleteCategory(int id);
    }
}
