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

namespace E_Commerce.Infrastructure.Repositories.Services
{
    public class WishlistRepository : GenericRepository<Wishlist>,IWishlistRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public WishlistRepository(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }


        public async Task<ResponseDto> GetWishlistById(int id)
        {
            var wishlist = await _context.Wishlists.Include(w => w.WishlistItems).FirstOrDefaultAsync(w=>w.WishlistID == id);
            var wishlistDto = _mapper.Map<ReturnWishlistDto>(wishlist);
            if (wishlist != null)
            {
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = wishlistDto
                };
            }
            


            return new ResponseDto
            {
                StatusCode = 404,
                IsSucceeded = false,
                DisplayMessage = "wishlist not found."
            };
        }

        public async Task<ResponseDto> GetWishlistByUserId(string userId)
        {
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

            var wishlist = await _context.Wishlists
                .Include(w => w.WishlistItems)
                .Where(c => c.UserID == userId).ToListAsync();
            var wishlistDto = _mapper.Map<List<ReturnWishlistDto>>(wishlist);
            if (wishlist != null)
            {
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = wishlistDto
                };
            }

            return new ResponseDto
            {
                StatusCode = 404,
                IsSucceeded = false,
                DisplayMessage = "wishlist not found for this user."
            };
        }
        public async Task<ResponseDto> GetPopularWishProducts()
        {
            var popularProducts = await _context.WishlistItems
                .Include(c => c.Product)
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




        public async Task<ResponseDto> CreateWishlist(WishlistDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserID);
            if (user == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "User not found"                   
                };
            }
            var wishlist = _mapper.Map<Wishlist>(dto);
            wishlist.AppUser = user;
            await _context.Wishlists.AddAsync(wishlist);
            var effectedRows = await _context.SaveChangesAsync();
            if (effectedRows > 0)
            {
                return new ResponseDto
                {
                    StatusCode = 201,
                    IsSucceeded = true,
                    Result = wishlist
                };
            }
            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "Can't add Cart."
            };
        }
        public async Task<ResponseDto> UpdateWishlist(int id, WishlistDto dto)
        {
            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Wishlist not found."
                };
            }
            var user = await _userManager.FindByIdAsync(dto.UserID);
            if (user == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "User not found"
                };
            }

            _mapper.Map(dto, wishlist);
            _context.Wishlists.Update(wishlist);
            var effectedRows = await _context.SaveChangesAsync();
            if (effectedRows > 0)
            {
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = wishlist
                };
            }
            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "Can't Update Wishlist."
            };
        }
        public async Task<ResponseDto> ClearWishlist(int id)
        {
            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist  == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Wishlist not found."
                };
            }

            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();

            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = wishlist
            };
        }

        
    }
}
