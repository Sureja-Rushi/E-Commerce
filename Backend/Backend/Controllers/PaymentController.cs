using Microsoft.AspNetCore.Mvc;
using Backend.Services;
using Backend.Helpers;
using Stripe;
using Stripe.Checkout;
//using Stripe.EventConstants;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Backend.DTOs;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _config;

        public PaymentController(IPaymentService paymentService, IConfiguration config)
        {
            _paymentService = paymentService;
            _config = config;
        }

        [HttpPost("{orderId}/initiate")]
        public async Task<IActionResult> InitiatePayment(int orderId, [FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            try
            {
                //var userId = GetUserIdFromToken();
                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    return Unauthorized(new { message = "Authorization token is not provided or invalid." });
                }

                // Extract the JWT token from the Authorization header
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                var userId = JwtTokenHelper.GetUserFromToken(token).Id;
                if (userId == null)
                    return Unauthorized(new { error = "User not authenticated" });

                var sessionUrl = await _paymentService.CreatePaymentSessionAsync(orderId, userId);
                return Ok(new { url = sessionUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("callback")]
        public async Task<IActionResult> HandleCallback([FromBody] CallbackRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.SessionId))
            {
                return BadRequest(new { error = "SessionId is required." });
            }

            try
            {
                await _paymentService.HandlePaymentCallbackAsync(request.SessionId);
                return Ok(new { message = "Payment processed successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{paymentId}/status")]
        public async Task<IActionResult> GetPaymentStatus(int paymentId)
        {
            var status = await _paymentService.GetPaymentStatusAsync(paymentId);
            return Ok(new { status });
        }

        [HttpGet("success")]
        public IActionResult PaymentSuccess([FromQuery] string session_id)
        {
            return Ok(new
            {
                message = "Payment successful! Thank you for your order.",
                sessionId = session_id
            });
        }

        [HttpGet("cancel")]
        public IActionResult PaymentCancelled()
        {
            return Ok(new
            {
                message = "Payment was cancelled. Please try again if needed."
            });
        }

        private int? GetUserIdFromToken()
        {
            var token = Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token)) return null;

            var user = JwtTokenHelper.GetUserFromToken(token);
            return user?.Id;
        }

        //private readonly IPaymentService _paymentService;

        //public PaymentController(IPaymentService paymentService)
        //{
        //    _paymentService = paymentService;
        //}

        //[HttpPost("initiate")] // Endpoint to initiate payment
        //public async Task<IActionResult> InitiatePayment([FromBody] PaymentRequest request)
        //{
        //    var session = await _paymentService.CreateCheckoutSessionAsync(request);
        //    return Ok(new { url = session.Url });
        //}

        //[HttpPost("webhook")] // Stripe webhook endpoint
        //public async Task<IActionResult> HandleWebhook()
        //{
        //    var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        //    try
        //    {
        //        var stripeEvent = _paymentService.ConstructStripeEvent(json, Request.Headers["Stripe-Signature"]);

        //        if (stripeEvent.Type == "checkout.session.completed") // Use the string literal
        //        {
        //            var session = stripeEvent.Data.Object as Session;
        //            if (session != null)
        //            {
        //                await _paymentService.HandleSuccessfulPaymentAsync(session);
        //            }
        //        }

        //        return Ok();
        //    }
        //    catch (StripeException e)
        //    {
        //        return BadRequest(new { error = e.Message });
        //    }
        //}
    }

    public class CallbackRequest
    {
        public string SessionId { get; set; }
    }

}
