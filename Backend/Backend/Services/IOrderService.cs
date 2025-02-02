using Backend.Models;

namespace Backend.Services
{
    public interface IOrderService
    {
        Task<(bool Success, string Message, int OrderId)> CreateOrderFromCartAsync(int userId, int? existingAddressId, Address newAddress = null);
        Task<bool> ChangeOrderStatusAsync(int orderId, string status);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetUserOrderHistoryAsync(int userId);
        Task<List<Order>> GetAllOrdersAsync();
        Task DeleteOrderByIdAsync(int orderId);

    }
}
