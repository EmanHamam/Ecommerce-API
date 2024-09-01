using E_Commerce.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Interfaces
{
    public interface IWishlistRepository
    {
        public Task<ResponseDto> GetWishlistByUserId(string userId);
        public Task<ResponseDto> GetWishlistById(int id);
        public Task<ResponseDto> GetPopularWishProducts();

        public Task<ResponseDto> CreateWishlist(WishlistDto dto);
        public Task<ResponseDto> UpdateWishlist(int id, WishlistDto dto);

        public Task<ResponseDto> ClearWishlist(int id);
    }
}
