using AutoMapper;
using E_Commerce.Domain.DTOs;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories.Services
{
    public class PaymentServices : GenericRepository<Payment>, IPaymentServices
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly ICartRepository _cartRepository;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public PaymentServices(ApplicationDbContext context, IConfiguration config, ICartRepository cartRepository, IMapper mapper, IOrderService orderService) : base(context)
        {
            _context = context;
            _config = config;
            _cartRepository = cartRepository;
            _mapper = mapper;
            _orderService = orderService;
        }
        public async Task<ResponseDto> CreateOrUpdatePaymentIntent(int cartId)
        {
            StripeConfiguration.ApiKey = _config["Stripe:Secretkey"];

            var cart = await _context.ShoppingCarts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefaultAsync(c => c.ShoppingCartID == cartId);

            if (cart == null)
            {
                return new ResponseDto
                {
                    StatusCode = 404,
                    IsSucceeded = false,
                    DisplayMessage = "Cart not found."
                };
            }

            var shippingPrice = 0m;

            if (cart.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _context.DeliveryMethod.FirstOrDefaultAsync(d => d.DeliveryMethodId == cart.DeliveryMethodId);
                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in cart.CartItems)
            {
                var productItem = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == item.ProductID);
                if (item.Price != productItem.Price)
                {
                    item.Price = productItem.Price;
                }
            }

            var service = new PaymentIntentService();

            PaymentIntent intent;

            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)cart.CartItems.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100,
                    Currency = "usd",

                    PaymentMethodTypes = new List<string> { "card" }
                };
                intent = await service.CreateAsync(options);
                cart.PaymentIntentId = intent.Id;
                cart.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)cart.CartItems.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100
                };
                await service.UpdateAsync(cart.PaymentIntentId, options);
            }
            var cartDto = _mapper.Map<UpdateCartDto>(cart);
            await _cartRepository.UpdateCart(cart.ShoppingCartID, cartDto);

            return new ResponseDto
            {
                Result=cart,
                IsSucceeded=true,
                StatusCode=200
             
            };
        }

        public async Task<ResponseDto> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var response = await _orderService.UpdateOrderStatus(paymentIntentId, "PaymentFailed");
            if (!response.IsSucceeded)
            {
                // Handle failure, log error, etc.
                // Optionally, you can return or throw an exception here based on your requirements
            }
            return response;
        }

        public async Task<ResponseDto> UpdateOrderPaymentSucceeded(string paymentIntentId)
        {
            var response = await _orderService.UpdateOrderStatus(paymentIntentId, "PaymentSucceeded");
            if (!response.IsSucceeded)
            {
                // Handle failure, log error, etc.
                // Optionally, you can return or throw an exception here based on your requirements
            }
            return response;
        }































    }
}
