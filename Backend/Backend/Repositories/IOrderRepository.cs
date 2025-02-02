using Backend.Models;

namespace Backend.Repositories
{
    public interface IOrderRepository
    {
        Task<bool> CreateOrderFromCartAsync(Order order, int userId, Address newAddress = null);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
        Task<List<Order>> GetUserOrderHistoryAsync(int userId);
        Task<List<Order>> GetAllOrdersAsync();
        Task DeleteOrderAsync(Order order);
        Task<bool> UpdateOrderPaymentStatusAsync(int orderId, string status);
    }
}
