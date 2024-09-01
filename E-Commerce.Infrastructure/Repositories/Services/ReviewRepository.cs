using AutoMapper;
using E_Commerce.Domain.DTOs;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Services
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewRepository(ApplicationDbContext context,
            IMapper mapper,UserManager<ApplicationUser> userManager) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _userManager=userManager;


        }

        public async Task<ResponseDto> GetAllReviews()
        {
            var reviews =await _context.Reviews.Include(r=>r.Product).ToListAsync();
            if(!reviews.Any())
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = "There are no Reviews"

                };
                
                    
            }
            var result = _mapper.Map<List<ReviewDto>>(reviews);
            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = result

            };

        }
        public async Task<ResponseDto> GetReviewById(int id)
        {
            var review = await _context.Reviews
                .Where(r=>r.ReviewID==id)
                .Include(r=>r.Product)
                .FirstOrDefaultAsync();
            if (review==null)
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = $"Review with ID {id} doesn't exist"

                };


            }

            var result = _mapper.Map<ReviewDto>(review);
            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = result

            };
        }
        public async Task<ResponseDto> GetProductReviews(int prdId)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductID == prdId);
            if (product == null)
            {
                return new ResponseDto
                {
                    StatusCode = 400,
                    IsSucceeded = false,
                    DisplayMessage = $"Product with ID {prdId} doesn't exist"
                };
            }

            var reviews = await _context.Reviews
                .Where(r => r.ProductID == prdId)
                .Include(r => r.Product)
                .ToListAsync();

            if (reviews.Any())
            {
         
                var averageRating = reviews.Average(r => r.Rating);

                
                var result = _mapper.Map<List<ReviewDto>>(reviews);

                result.ForEach(dto =>
                {
                    
                    dto.numOfReviews = reviews.Count;  
                });

                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = result
                };
            }

            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "There are no Reviews for this Product."
            };
        }
        public async Task<ResponseDto> GetProductReviewsSummary(int prdId)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductID == prdId);
            if (product == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = $"Product with ID {prdId} doesn't exist"
                };
            }

            var reviews = await _context.Reviews
                .Where(r => r.ProductID == prdId)
                .ToListAsync();

            if (reviews.Any())
            {
                var averageRating = reviews.Average(r => r.Rating);
                var numOfReviews = reviews.Count;

                var reviewSummaryDto = new ProductReviewSummaryDto
                {
                    ProductID = prdId,
                    AverageRating = averageRating,
                    NumOfReviews = numOfReviews
                };

                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = reviewSummaryDto
                };
            }

            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "There are no Reviews for this Product."
            };
        }
        public async Task<ResponseDto> GetUserReviews(string userID)
        {
            var reviews = await _context.Reviews
                .Where(r => r.AppUserID == userID)
                .Include(r => r.Product)
                .ToListAsync();
            if(reviews.Any())
            {
                var result = _mapper.Map<List<ReviewDto>>(reviews);
                result.ForEach(dto =>
                {
                    dto.numOfReviews = reviews.Count;
                });

                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = result
                };

            }
            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "There are no Reviews for this User."
            };
        }
        public async Task<ResponseDto> GetUsererReviewForProduct(string userId, int prdId)
        {
            var Product = await _context.Products
                .Where(p => p.ProductID == prdId)
               .FirstOrDefaultAsync();
            if (Product == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = $"Product with ID {prdId} doesn't exist"
                };
            }
            var reviews = await _context.Reviews.Where(r => r.AppUserID == userId)
                .Include(r => r.Product)
                .Where(p => p.ProductID == prdId)
                .ToListAsync();
            if (reviews != null && reviews.Count > 0)
            {
                var result = _mapper.Map<List<ReviewDto>>(reviews);
                result.ForEach(dto =>
                {
                    dto.numOfReviews = reviews.Count;
                });
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = result
                };
            }
            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = $"There are no Reviews by this User for Product with ID{prdId}"
            };
        }
        public async Task<ResponseDto> GetTopRatedProducts(int topN)
        {
            var products = await _context.Products
                .Include(p => p.Reviews)
                .Where(p => p.Reviews.Any())
                .Select(p => new
                {
                    Product = p,
                    AverageRating = p.Reviews.Average(r => r.Rating)
                })
                .OrderByDescending(p => p.AverageRating)
                .Take(topN)
                .ToListAsync();

            if (!products.Any())
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "No top-rated products found."
                };
            }

            var result = products.Select(p => new
            {
                p.Product.ProductID,
                p.Product.ProductName,
                AverageRating = p.AverageRating
            }).ToList();

            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = result
            };
        }
        public async Task<ResponseDto> GetRecentReviewsForAllProducts(int count)
        {

            var recentReviews = await _context.Reviews
                .Include(r => r.Product)
                .OrderByDescending(r => r.ReviewDate)
                .Take(count)
                .ToListAsync();

            if (!recentReviews.Any())
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "No recent reviews found."
                };
            }

            var result = _mapper.Map<List<ReviewDto>>(recentReviews);

            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = result
            };
        }
        public async Task<ResponseDto> GetRecentReviewsForProduct(int prdId, int count)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductID == prdId);
            if (product == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = $"Product with ID {prdId} doesn't exist"
                };
            }
            var recentReviews = await _context.Reviews
                .Where(r => r.ProductID == prdId)
                .Include(r => r.Product)
                .OrderByDescending(r => r.ReviewDate)
                .Take(count)
                .ToListAsync();

            if (!recentReviews.Any())
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "No recent reviews found For This Product."
                };
            }

            var result = _mapper.Map<List<ReviewDto>>(recentReviews);

            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result = result
            };
        }



        public async Task<ResponseDto> CreateReview(CreateReviewDto reviewDto)
        {
            var prd=_context.Products.Any(p=>p.ProductID== reviewDto.ProductID);
            var user = await _userManager.FindByIdAsync(reviewDto.AppUserID);

            if (!prd||user==null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = $"Invaild ProductID OR Invaild UserId"
                };
            }

            var review = _mapper.Map<Review>(reviewDto);
            review.ReviewDate = DateTime.UtcNow; 
            review.Product= await _context.Products.FindAsync(reviewDto.ProductID);
            await _context.Reviews.AddAsync(review);
           
            var effectedRows = await _context.SaveChangesAsync();
            if (effectedRows > 0)
            {
                return new ResponseDto
                {
                    StatusCode = 201,
                    IsSucceeded = true,
                    Result = review
                };
            }
            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "Can't add Review."
            };

        }
        public async Task<ResponseDto> UpdateReview(int id, CreateReviewDto reviewDto)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Review not found."
                };
            }
            var prd = _context.Products.Any(p => p.ProductID == reviewDto.ProductID);
            var user = await _userManager.FindByIdAsync(reviewDto.AppUserID);

            if (!prd || user == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = $"You must Enter vaild ProductID and vaild UserId"
                };
            }
            _mapper.Map(reviewDto, review);
            _context.Reviews.Update(review);
            var effectedRows = await _context.SaveChangesAsync();
            if (effectedRows > 0)
            {
                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSucceeded = true,
                    Result = review
                };
            }
            return new ResponseDto
            {
                StatusCode = 400,
                IsSucceeded = false,
                DisplayMessage = "Can't update Review."
            };

            
        }
        public async Task<ResponseDto> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Review not found."
                };
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return new ResponseDto
            {
                StatusCode = 200,
                IsSucceeded = true,
                Result=review
            };
        }



    }
}
