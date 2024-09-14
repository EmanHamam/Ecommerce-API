using E_Commerce.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Interfaces
{
    public interface ICartItemRepository
    {
        public  Task<ResponseDto> GetCartItem(int itemId);
        public  Task<ResponseDto> GetItemsInCart(int cartId);
        public Task<ResponseDto> GetItemsInCartByUserId(string userId);

        public Task<ResponseDto> AddItemToCart(CartItemDto itemDto);
        public  Task<ResponseDto> UpdateCartItem(int itemId, CartItemDto itemDto);
        public  Task<ResponseDto> RemoveCartItem(int itemId);
        public  Task<ResponseDto> RemoveCartItemByProductId(int cartId, int prdId);


    }
}
