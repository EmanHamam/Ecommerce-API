using E_Commerce.Domain.DTOs;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using E_Commerce.Infrastructure.Repositories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentServiceController : ControllerBase
    {
        private readonly string _whSecret;
        private readonly IPaymentServices _paymentService;
        private readonly ILogger<PaymentServiceController> _logger;
        private ResponseDto _responseDTO;

        public PaymentServiceController(IPaymentServices paymentService, ILogger<PaymentServiceController> logger, IConfiguration config)
        {
            _responseDTO = new ResponseDto();
            _paymentService = paymentService;
            _whSecret = config.GetSection("Stripe:WhSecret").Value;
            _logger = logger;
        }
        [Authorize]
        [HttpPost("{cartId}")]
        public async Task<ActionResult<ResponseDto>> CreateOrUpdatePaymentIntent(int cartId)
        {

            if (ModelState.IsValid)
            {
                _responseDTO = await _paymentService.CreateOrUpdatePaymentIntent(cartId);
                if (_responseDTO.IsSucceeded)
                {
                    return StatusCode(_responseDTO.StatusCode, _responseDTO.Result);
                }
                return StatusCode(_responseDTO.StatusCode, _responseDTO.DisplayMessage);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            var stripeSignature = Request.Headers["Stripe-Signature"];

            // Construct the Stripe event
            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, _whSecret);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to construct Stripe event: {Message}", ex.Message);
                return BadRequest("Invalid webhook signature.");
            }

            PaymentIntent intent;
            ResponseDto response;
            Order order = null;

            try
            {
                switch (stripeEvent.Type)
                {
                    case "payment_intent.succeeded":
                        intent = (PaymentIntent)stripeEvent.Data.Object;
                        _logger.LogInformation("Payment succeeded: {PaymentIntentId}", intent.Id);
                        response = await _paymentService.UpdateOrderPaymentSucceeded(intent.Id);

                        if (response.IsSucceeded)
                        {
                            order = (Order)response.Result;
                            _logger.LogInformation("Order updated to payment succeeded: {OrderId}", order.OrderID);
                        }
                        else
                        {
                            _logger.LogError("Failed to update order status to payment succeeded: {Message}", response.DisplayMessage);
                        }
                        break;

                    case "payment_intent.payment_failed":
                        intent = (PaymentIntent)stripeEvent.Data.Object;
                        _logger.LogInformation("Payment failed: {PaymentIntentId}", intent.Id);
                        response = await _paymentService.UpdateOrderPaymentFailed(intent.Id);

                        if (response.IsSucceeded)
                        {
                            order = (Order)response.Result;
                            _logger.LogInformation("Order updated to payment failed: {OrderId}", order.OrderID);
                        }
                        else
                        {
                            _logger.LogError("Failed to update order status to payment failed: {Message}", response.DisplayMessage);
                        }
                        break;

                    default:
                        _logger.LogInformation("Unhandled event type: {EventType}", stripeEvent.Type);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error handling Stripe event: {Message}", ex.Message);
                return StatusCode(500, "Internal server error");
            }

            return new EmptyResult();
        }


    }
}
