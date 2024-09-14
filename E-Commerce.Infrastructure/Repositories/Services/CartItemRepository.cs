using AutoMapper;
using E_Commerce.Domain.DTOs;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StackExchange.Redis.Role;

namespace E_Commerce.Infrastructure.Repositories.Services
{
    public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CartItemRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetCartItem(int itemId)
        {
            var cartItem = await _context.CartItems.Include(ci => ci.Product)
                               .FirstOrDefaultAsync(ci => ci.CartItemID == itemId);
            if (cartItem == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Cart item not found."
                };
            }
                var cartItemDto = _mapper.Map<ReturnCartItemDto>(cartItem);
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = cartItemDto
                };
        }
        public async Task<ResponseDto> GetItemsInCartByUserId(string userId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserID == userId);
        
            if (cart == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Cart not found."
                };
            }
        
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(c => c.ShoppingCartID == cart.ShoppingCartID)
                .ToListAsync();
        
            if (cartItems.Any())
            {
                var cartItemsDto = _mapper.Map<List<ReturnCartItemDto>>(cartItems);
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = cartItemsDto
                };
            }
        
            return new ResponseDto
            {
                StatusCode = 404,
                IsSucceeded = false,
                DisplayMessage = "There are no items in this cart."
            };
        }

        public async Task<ResponseDto> GetItemsInCart(int cartId)
        {
            var cart = await _context.ShoppingCarts.Include(c => c.CartItems)
                               .FirstOrDefaultAsync(c => c.ShoppingCartID == cartId);
            if (cart == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Cart not found."
                };
            }
            var cartItems = await _context.CartItems.Include(ci => ci.Product).Where(c => c.ShoppingCartID == cartId).ToListAsync();

            if (cartItems.Any())
            {
                var cartItemsDto = _mapper.Map<List<ReturnCartItemDto>>(cartItems);
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = cartItemsDto
                };
            }

            return new ResponseDto
            {
                StatusCode = 404,
                IsSucceeded = false,
                DisplayMessage = "There is no items in this cart."
            };
        }
        public async Task<ResponseDto> AddItemToCart(CartItemDto itemDto)
        {
            var checkResult = await CheckCartAndProductExistence(itemDto.ShoppingCartID, itemDto.ProductID);
            if (!checkResult.IsSucceeded)
            {
                return checkResult;
            }

            var cart = checkResult.Result as ShoppingCart;

            // Check if the item already exists in the cart
            var existingCartItem = cart!.CartItems!
                .FirstOrDefault(ci => ci.ProductID == itemDto.ProductID);

            if (existingCartItem != null)
            {
                // Increase the quantity if the item already exists
                existingCartItem.Quantity += itemDto.Quantity;
                existingCartItem.Price = existingCartItem.Quantity * existingCartItem!.Product!.Price;

            }

            else
            {
                var cartItem = _mapper.Map<CartItem>(itemDto);
                
                cartItem.Product = await _context.Products.FindAsync(itemDto.ProductID);
                cartItem.Price = cartItem.Quantity * cartItem.Product.Price;
                cart.CartItems.Add(cartItem);
            }
            var effectedRows = await _context.SaveChangesAsync();
            if (effectedRows > 0)
            {
                return new ResponseDto
                {
                    StatusCode = 201,
                    IsSucceeded = true,
                    Result = existingCartItem ?? cart.CartItems.Last()
                };
            }


            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "Can't add item to cart."
            };
        }

        public async Task<ResponseDto> UpdateCartItem(int itemId, CartItemDto itemDto)
        {
            var cartItem = await _context.CartItems.FindAsync(itemId);
           
            if (cartItem == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Cart item not found."
                };
            }
            var checkResult = await CheckCartAndProductExistence(itemDto.ShoppingCartID, itemDto.ProductID);
            if (!checkResult.IsSucceeded)
            {
                return checkResult;
            }

            _mapper.Map(itemDto, cartItem);
            _context.CartItems.Update(cartItem);
            var effectedRows = await _context.SaveChangesAsync();

            if (effectedRows > 0)
            {
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = _mapper.Map<CartItemDto>(cartItem)
                };
            }

            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "Can't update cart item."
            };
        }

        public async Task<ResponseDto> RemoveCartItem(int itemId)
        {
            var cartItem = await _context.CartItems.FindAsync(itemId);
            if (cartItem == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Cart item not found."
                };
            }

            _context.CartItems.Remove(cartItem);
            var effectedRows = await _context.SaveChangesAsync();

            if (effectedRows > 0)
            {
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    DisplayMessage = "Cart item removed successfully."
                };
            }

            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "Can't remove cart item."
            };
        }
        public async Task<ResponseDto> RemoveCartItemByProductId(int cartId,int prdId)
        {
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(c => c.ProductID == prdId&&c.ShoppingCartID==cartId);

            if (cartItem == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Cart item not found."
                };
            }

            _context.CartItems.Remove(cartItem);
            var effectedRows = await _context.SaveChangesAsync();

            if (effectedRows > 0)
            {
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    DisplayMessage = "Cart item removed successfully."
                };
            }

            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "Can't remove cart item."
            };
        }

        private async Task<ResponseDto> CheckCartAndProductExistence(int cartId, int productId)
        {
            var cart = await _context.ShoppingCarts.Include(c => c.CartItems).ThenInclude(ci=>ci.Product)
                .FirstOrDefaultAsync(c => c.ShoppingCartID == cartId);

            if (cart == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Cart not found."
                };
            }

            var productExists = await _context.Products.AnyAsync(p => p.ProductID == productId);
            if (!productExists)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = $"Product with ID {productId} doesn't exist."
                };
            }

            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = cart
            };
        }

    }
}
