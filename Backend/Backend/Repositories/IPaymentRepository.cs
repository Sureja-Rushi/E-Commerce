using Backend.Models;

namespace Backend.Repositories
{
    public interface IPaymentRepository
    {
        Task CreatePaymentAsync(int orderId, int userId, decimal amount, string currency, string paymentGateway, string transactionId, string status);
        Task<Payment> GetPaymentBySessionIdAsync(string sessionId);
        Task<Payment> GetPaymentByIdAsync(int paymentId);
        Task UpdatePaymentStatusAsync(int paymentId, string status);

        //Task UpdatePaymentStatusByTransactionIdAsync(string transactionId, string status);
    }
}
