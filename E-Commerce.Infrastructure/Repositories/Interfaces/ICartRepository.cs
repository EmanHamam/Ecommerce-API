using E_Commerce.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Interfaces
{
    public interface ICartRepository
    {
        public Task<ResponseDto> GetCartByUser();
        public Task<ResponseDto> GetCartById(int id);
        public Task<ResponseDto> GetPopularSalledProducts();

        //public Task<ResponseDto> CreateCart(CartDto dto);
        public Task<ResponseDto> CreateCart();

        public Task<ResponseDto> UpdateCart(int id, UpdateCartDto dto);
        public Task<ResponseDto> RemoveFromCart(int cartitemId);
        public Task<ResponseDto> ClearCart(int id);

    }
}
