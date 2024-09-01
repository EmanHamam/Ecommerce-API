using E_Commerce.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Interfaces
{
    public interface IWishlistItemRepository
    {
        public Task<ResponseDto> GetWishlistItem(int itemId);
        public Task<ResponseDto> GetWishlistItems(int cartId);
        public Task<ResponseDto> AddItemToWishlist(WishlistItemDto itemDto);
        public Task<ResponseDto> UpdateWishlistItem(int itemId, WishlistItemDto itemDto);
        public Task<ResponseDto> RemoveWishlistItem(int itemId);

    }
}
