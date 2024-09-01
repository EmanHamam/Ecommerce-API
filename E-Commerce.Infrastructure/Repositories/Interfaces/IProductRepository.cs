using E_Commerce.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Interfaces
{
    public interface IProductRepository
    {
        //public Task<ResponseDto> GetAllProducts();
        public Task<ResponseDto> GetAllProducts(ProductParams? productParams);
        public Task<IEnumerable<int>> GetAllIDs();
        public Task<ResponseDto> GetProductById(int id);
        public Task<ResponseDto> GetProductDetailsById(int id);
        public Task<ResponseDto> GetProductsByCategoryId(int categoryId, ProductParams? productParams);
        public Task<ResponseDto> GetProductsByBrand(string brand);
       


        public Task<ResponseDto> AddProduct(CreateProductDto dto);
        public Task<ResponseDto> AddProductDetails(CreateProductDetailsDto dto);
        public Task<ResponseDto> UpdateProduct(int id, UpdateProductDto dto);
        public Task<ResponseDto> UpdateProductDetails(int id, CreateProductDetailsDto dto);
        public Task<ResponseDto> DeleteProduct(int id);
    }
}
