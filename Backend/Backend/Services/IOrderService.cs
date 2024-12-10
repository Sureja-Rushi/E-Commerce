using Backend.DTOs;
using Backend.Models;

namespace Backend.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int userId, string shippingAddress);
        Task UpdateOrderStatusAsync(int orderId, string status, int adminUserId);
        Task<IQueryable<Order>> GetUserOrdersAsync(int userId);
        Task<OrderDetailsDto> GetOrderDetailsAsync(int orderId, int userId);
        Task<List<OrderDetailsDto>> GetOrdersByStatusAsync(string status);
    }
}
