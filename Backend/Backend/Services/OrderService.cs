using Backend.DTOs;
using Backend.Models;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICartService _cartService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly AppDbContext _context;
        private readonly IProductRepository productRepository;
        private readonly ICartRepository cartRepository;

        public OrderService(ICartService cartService, IUserService userService, IProductService productService, AppDbContext context)
        {
            _cartService = cartService;
            _userService = userService;
            _productService = productService;
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(int userId, string shippingAddress)
        {
            // Ensure the user exists
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            // Get cart for the user and ensure it is not empty
            var cartDto = await _cartService.GetCartAsync();
            if (cartDto == null || cartDto.CartItems == null || !cartDto.CartItems.Any())
                throw new Exception("Cart is empty.");

            // Validate product availability and calculate total amount
            decimal totalAmount = 0;
            foreach (var cartItem in cartDto.CartItems)
            {
                var productDto = await _productService.GetProductDetailsAsync(cartItem.ProductId);
                if (productDto == null)
                    throw new Exception($"Product with ID {cartItem.ProductId} not found.");

                if (productDto.StockQuantity < cartItem.Quantity)
                    throw new Exception($"Not enough stock for product {productDto.Name}. Available: {productDto.StockQuantity}, Required: {cartItem.Quantity}");

                totalAmount += productDto.Price * cartItem.Quantity;
            }

            // Create the order
            var order = new Order
            {
                UserId = userId,
                ShippingAddress = shippingAddress,
                TotalAmount = totalAmount,
                OrderStatus = "Pending",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            // Create order items and validate product details
            foreach (var cartItem in cartDto.CartItems)
            {
                var productDto = await _productService.GetProductDetailsAsync(cartItem.ProductId);
                if (productDto == null)
                    throw new Exception($"Product with ID {cartItem.ProductId} not found during order item creation.");

                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = productDto.Price,
                    TotalPrice = productDto.Price * cartItem.Quantity
                };
                await _context.OrderItems.AddAsync(orderItem);
            }

            await _context.SaveChangesAsync();

            // Deduct stock from inventory
            foreach (var cartItem in cartDto.CartItems)
            {
                var productDto = await _productService.GetProductDetailsAsync(cartItem.ProductId);
                if (productDto == null)
                    throw new Exception($"Product with ID {cartItem.ProductId} not found during stock update.");

                if (productDto.StockQuantity < cartItem.Quantity)
                    throw new Exception($"Insufficient stock for product {productDto.Name} during stock update.");

                await _productService.UpdateStockAsync(cartItem.ProductId, -cartItem.Quantity);
            }

            // Clear the cart after successful order placement
            await _cartService.RemoveCartItemAsync(cartDto.Id, true);

            return order;
        }



        public async Task<IQueryable<Order>> GetUserOrdersAsync(int userId)
        {
            return _context.Orders
            .Where(o => o.UserId == userId);
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status, int adminUserId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) throw new Exception("Order not found.");

            // Check if the user is an admin
            var user = await _userService.GetUserByIdAsync(adminUserId);
            if (user == null || user.Role != "Admin")
            {
                throw new UnauthorizedAccessException("Only admins can update the order status.");
            }

            order.OrderStatus = status;
            order.UpdatedAt = DateTime.UtcNow;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<OrderDetailsDto> GetOrderDetailsAsync(int orderId, int userId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product) // To include product details
                .FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null || order.UserId != userId)
                throw new UnauthorizedAccessException("You do not have access to this order.");

            return new OrderDetailsDto
            {
                OrderId = order.Id,
                ShippingAddress = order.ShippingAddress,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                Items = order.OrderItems.Select(item => new OrderItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name, // Assuming Product.Name is loaded
                    Quantity = item.Quantity,
                    Price = item.Price,
                    TotalPrice = item.TotalPrice
                }).ToList()
            };
        }

        public async Task<List<OrderDetailsDto>> GetOrdersByStatusAsync(string status)
        {
            // Fetch orders with the given status
            var orders = await _context.Orders
        .Where(o => o.OrderStatus.Equals(status, StringComparison.OrdinalIgnoreCase))
        .Include(o => o.OrderItems) // Include related data if needed
        .ToListAsync();

            if (orders == null || !orders.Any())
                throw new Exception("No orders found with the specified status.");

            // Map to DTOs if needed
            return orders.Select(o => new OrderDetailsDto
            {
                OrderId = o.Id,
                //UserId = o.UserId,
                ShippingAddress = o.ShippingAddress,
                TotalAmount = o.TotalAmount,
                OrderStatus = o.OrderStatus,
                //CreatedAt = o.CreatedAt,
                //UpdatedAt = o.UpdatedAt,
                Items = o.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    TotalPrice = oi.TotalPrice
                }).ToList()
            }).ToList();
        }

    }
}
