using E_Commerce.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        public Task<ResponseDto> GetAllReviews();
        public  Task<ResponseDto> GetProductReviews(int prdId);
        public Task<ResponseDto> GetProductReviewsSummary(int prdId);
        public Task<ResponseDto> GetReviewById(int id);
        public  Task<ResponseDto> GetUserReviews(string userID);
        public  Task<ResponseDto> GetUsererReviewForProduct(string userId, int prdId);
        public  Task<ResponseDto> GetTopRatedProducts(int topN);
        public Task<ResponseDto> GetRecentReviewsForAllProducts(int count);
        public Task<ResponseDto> GetRecentReviewsForProduct(int prdId, int count);
        public  Task<ResponseDto> CreateReview(CreateReviewDto reviewDto);
        public Task<ResponseDto> UpdateReview(int id, CreateReviewDto reviewDto);
        public  Task<ResponseDto> DeleteReview(int id);


    }
}
