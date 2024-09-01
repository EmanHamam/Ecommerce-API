using AutoMapper;
using E_Commerce.Domain.DTOs;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Services
{
    public class CartRepository : GenericRepository<ShoppingCart>, ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CartRepository(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseDto> GetCartByUser()
        {
            var userId = await GetCurrentUserAsync();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "User not found."
                };
            }

            var cart = await _context.ShoppingCarts
                .Include(c => c.CartItems).ThenInclude(ci => ci.Product)
                .Where(c => c.UserID == userId).ToListAsync();
            var cartDto = _mapper.Map<List<ReturnCartDto>>(cart);
            if (cart != null)
            {
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = cartDto
                };
            }

            return new ResponseDto
            {
                StatusCode = 404,
                IsSucceeded = false,
                DisplayMessage = "Cart not found for this user."
            };
        }

        public async Task<ResponseDto> GetCartById(int id)
        {
            var cart = await _context.ShoppingCarts.Include(c=>c.CartItems).ThenInclude(ci=>ci.Product).FirstOrDefaultAsync(c => c.ShoppingCartID ==id);
            if (cart != null)
            {
                var cartDto = _mapper.Map<ReturnCartDto>(cart);

                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = cartDto
                };
            }

            return new ResponseDto
            {
                StatusCode = 404,
                IsSucceeded = false,
                DisplayMessage = "Cart not found."
            };
        }

        public async Task<ResponseDto> GetPopularSalledProducts() //need Updates
        {
            var popularProducts = await _context.CartItems
                .Include(c=>c.Product)
                .GroupBy(c => c.ProductID)
                .OrderByDescending(g => g.Count())
                .Select(g => new ProductDto
                {
                    ProductID = g.Key,
                    Frequency = g.Count(),
                    

                })
                .Take(8) 
                .ToListAsync();

            if (!popularProducts.Any())
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "No popular products found."
                };
            }

            var productDtos = _mapper.Map<List<Product>>(popularProducts);

            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = popularProducts
            };
        }




        //public async Task<ResponseDto> CreateCart(CartDto dto)
        //{
        //    var currentUserId = await GetCurrentUserAsync();
        //    var user = await _userManager.FindByIdAsync(currentUserId);
        //    if (user == null)
        //    {
        //        return new ResponseDto
        //        {
        //            StatusCode = 404,
        //            IsSucceeded = false,
        //            DisplayMessage = "User not found"
        //        };
        //    }
        //    var cart = _mapper.Map<ShoppingCart>(dto);
        //    cart.UserID=currentUserId;
        //    await _context.ShoppingCarts.AddAsync(cart);
        //    var effectedRows = await _context.SaveChangesAsync();
        //    if (effectedRows > 0)
        //    {
        //        return new ResponseDto
        //        {
        //            StatusCode = 201,
        //            IsSucceeded = true,
        //            Result = cart
        //        };
        //    }
        //    return new ResponseDto
        //    {
        //        StatusCode = 400,
        //        IsSucceeded = false,
        //        DisplayMessage = "Can't add Cart."
        //    };

        //}
        public async Task<ResponseDto> CreateCart()
        {
            //var currentUserId = await GetCurrentUserAsync();
            //var user = await _userManager.FindByIdAsync(currentUserId);
            //if (user == null)
            //{
            //    return new ResponseDto
            //    {
            //        StatusCode = 404,
            //        IsSucceeded = false,
            //        DisplayMessage = "User not found"
            //    };
            //}
            var cart = new ShoppingCart();
            //cart.CartItems = _mapper.Map<List<CartItem>>(items);
            // cart.UserID = currentUserId;
            
            await _context.ShoppingCarts.AddAsync(cart);
            var effectedRows = await _context.SaveChangesAsync();
            if (effectedRows > 0)
            {
                return new ResponseDto
                {
                    StatusCode = 201,
                    IsSucceeded = true,
                    Result = _mapper.Map<CartDto>(cart)
                };
            }
            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "Can't add Cart."
            };

        }

        public async Task<ResponseDto> UpdateCart(int id, UpdateCartDto dto)
        {
            var cart = await _context.ShoppingCarts.Include(c => c.CartItems)
                   .FirstOrDefaultAsync(c => c.ShoppingCartID == id);
            if (cart == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Cart not found."
                };
            }
            var currentUserId = await GetCurrentUserAsync();
            var user = await _userManager.FindByIdAsync(currentUserId);
            if (cart.UserID != currentUserId)
            {
                return new ResponseDto
                {
                    StatusCode = 403,
                    IsSucceeded = false,
                    DisplayMessage = "Unauthorized to update this cart."
                };
            }

            //cart.CartItems = _mapper.Map<List<CartItem>>(dto.Items);
            _mapper.Map(dto, cart);
            _context.ShoppingCarts.Update(cart);
            var effectedRows = await _context.SaveChangesAsync();
            if (effectedRows > 0)
            {
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = cart
                };
            }
            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "Can't Update Cart."
            };
        }
        public async Task<ResponseDto> RemoveFromCart(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
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
                    DisplayMessage = "Item removed from cart successfully."
                };
            }
            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "Can't remove item from cart."
            };
        }

        public async Task<ResponseDto> ClearCart(int id)
        {
            var cart = await _context.ShoppingCarts.FindAsync(id);
            if (cart == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Cart not found."
                };
            }

            _context.ShoppingCarts.Remove(cart);
            await _context.SaveChangesAsync();

            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = cart
            };
        }
        private async Task<string> GetCurrentUserAsync()
        {
            var userClaim = _httpContextAccessor.HttpContext!.User;

            // Get the current user from claims
            var user = await _userManager.GetUserAsync(userClaim);

            return user.Id;
        }
    }
}
