using Backend.DTOs;
using Stripe;
using Stripe.Checkout;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentSessionAsync(int orderId, int userId);
        Task HandlePaymentCallbackAsync(string sessionId);
        Task<string> GetPaymentStatusAsync(int paymentId);
        //Task<Session> CreateCheckoutSessionAsync(PaymentRequest request);
        //Event ConstructStripeEvent(string json, string stripeSignature);
        //Task HandleSuccessfulPaymentAsync(Session session);
    }
}
