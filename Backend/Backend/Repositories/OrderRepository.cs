using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class OrderRepository : IOrderRepository
    {

        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateOrderFromCartAsync(Order order, int userId, Address newAddress = null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var cart = await _context.Carts
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || !cart.CartItems.Any())
                    throw new Exception("Cart is empty or does not exist.");

                if (newAddress != null)
                {
                    newAddress.UserId = userId;
                    _context.Addresses.Add(newAddress);
                    await _context.SaveChangesAsync();
                    order.ShippingAddressId = newAddress.Id;
                }

                foreach (var cartItem in cart.CartItems)
                {
                    var orderItem = new OrderItem
                    {
                        Order = order,
                        ProductId = cartItem.ProductId,
                        UserId = userId,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Price,
                        DiscountedPrice = cartItem.DiscountedPrice,
                        Size = cartItem.Size
                    };

                    //order.TotalPrice += cartItem.Price * cartItem.Quantity;
                    order.TotalPrice += cartItem.Price;
                    //order.TotalDiscountedPrice += cartItem.DiscountedPrice * cartItem.Quantity;
                    order.TotalDiscountedPrice += cartItem.DiscountedPrice;
                    order.TotalItems += cartItem.Quantity;

                    _context.OrderItems.Add(orderItem);
                }

                order.Discount = order.TotalPrice - order.TotalDiscountedPrice;

                // Add the order to the database
                _context.Orders.Add(order);

                // Clear the user's cart
                _context.CartItems.RemoveRange(cart.CartItems);
                cart.TotalPrice = 0;
                cart.TotalDiscountedPrice = 0;
                cart.TotalItems = 0;
                cart.Discount = 0;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }  
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.ShippingAddress)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<List<Order>> GetUserOrderHistoryAsync(int userId)
        {
            return await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.ShippingAddress)
            .Where(o => o.UserId == userId && o.OrderStatus == "PLACED")
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await GetOrderByIdAsync(orderId);

            if(order == null)
                return false;

            if(status.ToLower() == "placed")
            {
                order.OrderStatus = status.ToUpper();
                order.PaymentStatus = "PAID";
            }
            else
            {
                order.OrderStatus = status.ToUpper();
            }

            _context.Orders.Update(order);

            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.ShippingAddress)
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task DeleteOrderAsync(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateOrderPaymentStatusAsync(int orderId, string status)
        {
            var order = await GetOrderByIdAsync(orderId);

            if (order == null)
                return false;

            order.PaymentStatus = status.ToUpper();

            _context.Orders.Update(order);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
