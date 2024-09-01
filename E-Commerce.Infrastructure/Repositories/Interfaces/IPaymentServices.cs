using E_Commerce.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Interfaces
{
    public interface IPaymentServices
    {
        Task<ResponseDto> CreateOrUpdatePaymentIntent(int cartId);

        Task<ResponseDto> UpdateOrderPaymentSucceeded(string paymentIntentId);
        Task<ResponseDto> UpdateOrderPaymentFailed(string paymentIntentId);
    }
}
