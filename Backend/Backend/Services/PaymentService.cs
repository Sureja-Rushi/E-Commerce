using Backend.Repositories;
using Stripe.Checkout;
using Stripe;
using Backend.DTOs;

namespace Backend.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _config;
        private readonly IOrderService _orderService; // Assume this manages orders
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(IConfiguration config, IOrderService orderService, IPaymentRepository paymentRepository, IOrderRepository orderRepository)
        {
            _config = config;
            _orderService = orderService;
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<string> CreatePaymentSessionAsync(int orderId, int userId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new Exception("Invalid order details.");

            if (order.PaymentStatus == "Success")
                throw new Exception("Payment has already been completed for this order.");

            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmountDecimal = order.TotalDiscountedPrice * 100, // Stripe requires the amount in cents
                            Currency = "inr", // Change to your currency as needed
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Payment Requested from Trendy Fashions...",
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = $"http://localhost:5173/payment/{order.Id}",
                CancelUrl = "https://www.youtube.com",
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            if (session.AmountTotal == null)
                throw new Exception("Session total amount is not available.");

            await _paymentRepository.CreatePaymentAsync(orderId, userId, session.AmountTotal.Value / 100m, "USD", "Stripe", session.Id, "Pending");
            return session.Url;
        }

        public async Task HandlePaymentCallbackAsync(string sessionId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
            var service = new SessionService();
            var session = await service.GetAsync(sessionId);

            var payment = await _paymentRepository.GetPaymentBySessionIdAsync(sessionId);
            if (payment == null)
                throw new Exception("Payment record not found.");

            if (session.PaymentStatus == "paid")
            {
                await _paymentRepository.UpdatePaymentStatusAsync(payment.Id, "Success");
                await _orderRepository.UpdateOrderStatusAsync(payment.OrderId, "placed");
            }
            else
            {
                await _paymentRepository.UpdatePaymentStatusAsync(payment.Id, "Failed");
                await _orderRepository.UpdateOrderPaymentStatusAsync(payment.OrderId, "Failed");
            }
        }

        public async Task<string> GetPaymentStatusAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
            return payment?.Status ?? "Not Found";
        }

        //private readonly string _stripeSecretKey;
        //private readonly IPaymentRepository _paymentRepository;
        //private readonly IConfiguration configuration;

        //public PaymentService(IConfiguration configuration, IPaymentRepository paymentRepository)
        //{
        //    _stripeSecretKey = configuration["StripeSettings:SecretKey"];
        //    _paymentRepository = paymentRepository;
        //}

        //public async Task<Session> CreateCheckoutSessionAsync(PaymentRequest request)
        //{
        //    StripeConfiguration.ApiKey = _stripeSecretKey;

        //    var options = new SessionCreateOptions
        //    {
        //        PaymentMethodTypes = new List<string> { "card" },
        //        LineItems = new List<SessionLineItemOptions>
        //        {
        //            new SessionLineItemOptions
        //            {
        //                PriceData = new SessionLineItemPriceDataOptions
        //                {
        //                    UnitAmount = (long)(request.Amount * 100), // Amount in cents
        //                    Currency = request.Currency,
        //                    ProductData = new SessionLineItemPriceDataProductDataOptions
        //                    {
        //                        Name = "Order Payment"
        //                    }
        //                },
        //                Quantity = 1
        //            }
        //        },
        //        Mode = "payment",
        //        SuccessUrl = request.SuccessUrl,
        //        CancelUrl = request.CancelUrl
        //    };

        //    var service = new SessionService();
        //    var session = await service.CreateAsync(options);
        //    return session;
        //}

        //public Event ConstructStripeEvent(string json, string stripeSignature)
        //{
        //    return EventUtility.ConstructEvent(json, stripeSignature, configuration["StripeSettings:WebhookSecret"]);
        //}

        //public async Task HandleSuccessfulPaymentAsync(Session session)
        //{
        //    var paymentIntentId = session.PaymentIntentId;
        //    await _paymentRepository.UpdatePaymentStatusByTransactionIdAsync(paymentIntentId, "success");
        //}

    }
}
