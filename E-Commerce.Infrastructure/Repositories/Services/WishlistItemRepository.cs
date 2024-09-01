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
    public class WishlistItemRepository : GenericRepository<WishlistItem>, IWishlistItemRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public WishlistItemRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResponseDto> GetWishlistItem(int itemId)
        {
            var wishlistItem = await _context.WishlistItems.Include(wi => wi.Product)
                               .FirstOrDefaultAsync(wi => wi.WishlistItemID == itemId);
            if (wishlistItem == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "wishlist Items not found."
                };
            }
            var wishlistItemDto = _mapper.Map<ReturnWishlistItemDto>(wishlistItem);
            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = wishlistItemDto
            };
        }
        public async Task<ResponseDto> GetWishlistItems(int cartId)
        {
            var wishlist = await _context.Wishlists.Include(w => w.WishlistItems)
                               .FirstOrDefaultAsync(w => w.WishlistID == cartId);
            if (wishlist == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Cart not found."
                };
            }
            var wishlistItems = await _context.WishlistItems.Include(wi => wi.Product).Where(w=> w.WishlistID == cartId).ToListAsync();

            if (wishlistItems.Any())
            {
                var wishlistItemsDto = _mapper.Map<List<ReturnWishlistItemDto>>(wishlistItems);
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = wishlistItems
                };
            }

            return new ResponseDto
            {
                StatusCode = 404,
                IsSucceeded = false,
                DisplayMessage = "There is no items in this wishlist."
            };
        }
        public async Task<ResponseDto> AddItemToWishlist(WishlistItemDto itemDto)
        {
            var wishlist = await _context.Wishlists.Include(w=>w.WishlistItems)
                              .FirstOrDefaultAsync(w=>w.WishlistID==itemDto.WishlistID);
            var prd = await _context.Products.AnyAsync(p => p.ProductID == itemDto.ProductID);
            if (wishlist == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Wishlist not found."
                };
            }

            if (!prd)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = $"Product with ID {itemDto.ProductID} doesn't Exsit"
                };
            }

            var wishlistItem = _mapper.Map<WishlistItem>(itemDto);
            wishlistItem.Product = await _context.Products.FindAsync(itemDto.ProductID);
            wishlist.WishlistItems.Add(wishlistItem);

            var effectedRows = await _context.SaveChangesAsync();
            if (effectedRows > 0)
            {
                return new ResponseDto
                {
                    StatusCode = 201,
                    IsSucceeded = true,
                    Result = wishlistItem
                };
            }

            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "Can't add item to wishlist."
            };
        }

        public async Task<ResponseDto> UpdateWishlistItem(int itemId, WishlistItemDto itemDto)
        {
            var wishlistItem = await _context.WishlistItems.FindAsync(itemId);
            if (wishlistItem == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "wishlist item not found."
                };
            }
            var wishlist = await _context.Wishlists
                              .FirstOrDefaultAsync(w => w.WishlistID == itemDto.WishlistID);
            if (wishlist == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Wishlist not found."
                };
            }
            var prd = await _context.Products.AnyAsync(p => p.ProductID == itemDto.ProductID);
            if (!prd)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = $"Product with ID {itemDto.ProductID} doesn't Exsit"
                };
            }

            _mapper.Map(itemDto, wishlistItem);
            _context.WishlistItems.Update(wishlistItem);
            var effectedRows = await _context.SaveChangesAsync();

            if (effectedRows > 0)
            {
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = _mapper.Map<WishlistItemDto>(wishlistItem)
                };
            }

            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "Can't update wishlist item."
            };
        }

        public async Task<ResponseDto> RemoveWishlistItem(int itemId)
        {
            var wishlistItem = await _context.WishlistItems.FindAsync(itemId);
            if (wishlistItem == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "wishlist item not found."
                };
            }

            _context.WishlistItems.Remove(wishlistItem);
            var effectedRows = await _context.SaveChangesAsync();

            if (effectedRows > 0)
            {
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    DisplayMessage = "wishlist item removed successfully."
                };
            }

            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "Can't remove wishlist Item."
            };
        }
    }
}
