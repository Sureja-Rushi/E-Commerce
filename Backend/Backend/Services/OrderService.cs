using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class OrderService : IOrderService
    {

        private readonly IOrderRepository orderRepository;
        private readonly IAddressRepository addressRepository;

        public OrderService(IOrderRepository orderRepository, IAddressRepository addressRepository)
        {
            this.orderRepository = orderRepository;
            this.addressRepository = addressRepository;
        }

        public async Task<bool> ChangeOrderStatusAsync(int orderId, string status)
        {
            var validStatuses = new[] { "placed", "confirmed", "shipped", "delivered", "cancled" };

            if(!validStatuses.Contains(status.ToLower()))
                throw new ArgumentException("Invalid order status.");

            return await orderRepository.UpdateOrderStatusAsync(orderId, status);
        }

        public async Task<(bool Success, string Message, int OrderId)> CreateOrderFromCartAsync(int userId, int? existingAddressId, Address newAddress = null)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user ID.");

            Address shippingAddress = null;

            if (existingAddressId.HasValue)
            {
                // Fetch the existing address
                shippingAddress = await addressRepository.GetAddressByIdAsync(existingAddressId.Value);
                if (shippingAddress == null || shippingAddress.UserId != userId)
                    throw new Exception("Invalid or unauthorized address.");
            }
            else if (newAddress != null)
            {
                // Use the new address
                shippingAddress = newAddress;
            }
            else
            {
                throw new Exception("An address must be provided.");
            }

            var order = new Order
            {
                UserId = userId,
                ShippingAddressId = shippingAddress?.Id ?? 0
            };

            var success = await orderRepository.CreateOrderFromCartAsync(order, userId, newAddress);

            if (success)
            {
                return (true, "Order created successfully.", order.Id);
            }

            return (false, "Failed to create order.", 0);
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order = await orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");

            return order;
        }

        public async Task<List<Order>> GetUserOrderHistoryAsync(int userId)
        {
            var orders = await orderRepository.GetUserOrderHistoryAsync(userId);
            if (orders == null || !orders.Any())
            {
                throw new KeyNotFoundException("No orders found for the user.");
            }

            return orders;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            var orders = await orderRepository.GetAllOrdersAsync();
            if (orders == null || !orders.Any())
            {
                throw new KeyNotFoundException("No orders found.");
            }

            return orders;
        }

        public async Task DeleteOrderByIdAsync(int orderId)
        {
            var order = await orderRepository.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            }

            await orderRepository.DeleteOrderAsync(order);
        }
    }
}
