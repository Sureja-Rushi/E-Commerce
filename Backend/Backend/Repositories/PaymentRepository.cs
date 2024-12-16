using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        //public async Task CreatePaymentAsync(int orderId, int userId, decimal amount, string currency, string paymentGateway, string transactionId, string status)
        //{
        //    try
        //    {
        //        var payment = new Payment
        //        {
        //            OrderId = orderId,
        //            UserId = userId,
        //            Amount = amount,
        //            Currency = currency,
        //            PaymentGateway = paymentGateway,
        //            TransactionId = transactionId,
        //            Status = status,
        //            CreatedAt = DateTime.UtcNow,
        //            UpdatedAt = DateTime.UtcNow
        //        };

        //        _context.Payments.Add(payment);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        var innerException = ex.InnerException?.Message ?? ex.Message;
        //        Console.WriteLine($"Database error: {innerException}");
        //        throw new Exception($"A database error occurred: {innerException}");
        //    }
        //}



        //public async Task<Payment> GetPaymentBySessionIdAsync(string sessionId)
        //{
        //    // Assume that the SessionId is stored in the TransactionId column
        //    return await _context.Payments
        //        .FirstOrDefaultAsync(p => p.TransactionId == sessionId);
        //}

        //public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        //{
        //    return await _context.Payments
        //        .FirstOrDefaultAsync(p => p.Id == paymentId);
        //}

        //public async Task UpdatePaymentStatusAsync(int paymentId, string status)
        //{
        //    var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);
        //    if (payment != null)
        //    {
        //        payment.Status = status;
        //        payment.UpdatedAt = DateTime.UtcNow;
        //        _context.Payments.Update(payment);
        //        await _context.SaveChangesAsync();
        //    }
        //}

        public async Task CreatePaymentAsync(int orderId, int userId, decimal amount, string currency, string paymentGateway, string transactionId, string status)
        {
            try
            {
                var payment = new Payment
                {
                    OrderId = orderId,
                    UserId = userId,
                    Amount = amount,
                    Currency = currency,
                    PaymentGateway = paymentGateway,
                    TransactionId = transactionId,
                    Status = status,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException?.Message ?? ex.Message;
                Console.WriteLine($"Database error: {innerException}");
                throw new Exception($"A database error occurred: {innerException}");
            }
        }

        public async Task<Payment> GetPaymentBySessionIdAsync(string sessionId)
        {
            // Fetch payment by the correct TransactionId (sessionId)
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.TransactionId == sessionId);
        }

        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.Id == paymentId);
        }

        public async Task UpdatePaymentStatusAsync(int paymentId, string status)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);
            if (payment != null)
            {
                payment.Status = status;
                payment.UpdatedAt = DateTime.UtcNow;
                _context.Payments.Update(payment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> PaymentExistsAsync(string transactionId)
        {
            return await _context.Payments.AnyAsync(p => p.TransactionId == transactionId);
        }

        //private readonly AppDbContext _context;

        //public PaymentRepository(AppDbContext context)
        //{
        //    _context = context;
        //}

        //public async Task UpdatePaymentStatusByTransactionIdAsync(string transactionId, string status)
        //{
        //    var payment = await _context.Payments.FirstOrDefaultAsync(p => p.TransactionId == transactionId);

        //    if (payment != null)
        //    {
        //        payment.Status = status;
        //        payment.UpdatedAt = DateTime.UtcNow;
        //        _context.Payments.Update(payment);
        //        await _context.SaveChangesAsync();
        //    }
        //}

    }
}
