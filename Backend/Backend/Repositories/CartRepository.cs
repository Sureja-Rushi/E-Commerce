using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository userRepository;

        public CartRepository(AppDbContext context, IUserRepository userRepository)
        {
            _context = context;
            this.userRepository = userRepository;
        }

        public async Task<bool> CreateCartAsync(int userId)
        {
            if (await CartExistsAsync(userId))
            {
                throw new InvalidOperationException("Cart already exists for this user.");
            }

            var cart = new Cart
            {
                UserId = userId,
                TotalPrice = 0,
                TotalDiscountedPrice = 0,
                TotalItems = 0,
                Discount = 0,
                User = await userRepository.GetUserByIdAsync(userId)
            };

            _context.Carts.Add(cart);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CartExistsAsync(int userId)
        {
            return await _context.Carts.AnyAsync(c => c.UserId == userId);
        }

        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    //.ThenInclude(p => p.Reviews)
                //.Include(c => c.CartItems)
                //    .ThenInclude(ci => ci.Size)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<bool> AddCartItemAsync(int userId, int productId, string sizeName)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if(cart == null)
            {
                throw new Exception("Cart not found for this user");
            }

            var product = await _context.Products
                .Include(p => p.Sizes)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if(product == null)
            {
                throw new Exception("Product not found");
            }

            var productSize = product.Sizes.FirstOrDefault(s => s.SizeName.Equals(sizeName, StringComparison.OrdinalIgnoreCase));
            if(productSize == null)
            {
                throw new Exception("Size not found for this product");
            }

            var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId && ci.Size == productSize.SizeName);

            if(existingCartItem != null)
            {
                existingCartItem.Quantity += 1;
                existingCartItem.Price += product.Price;
                existingCartItem.DiscountedPrice += product.DiscountedPrice;
            }
            else
            {
                var newCartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Size = sizeName,
                    Quantity = 1,
                    Price = product.Price,
                    DiscountedPrice = product.DiscountedPrice,
                    Cart = cart,
                    Product = product,
                    UserId = userId,
                    User = cart.User
                };

                _context.CartItems.Add(newCartItem);
            }

            cart.TotalItems = cart.CartItems.Sum(ci => ci.Quantity);
            //cart.TotalPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.Price);
            cart.TotalPrice = cart.CartItems.Sum(ci => ci.Price);
            //cart.TotalDiscountedPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.DiscountedPrice);
            cart.TotalDiscountedPrice = cart.CartItems.Sum(ci => ci.DiscountedPrice);
            cart.Discount = cart.TotalPrice - cart.TotalDiscountedPrice;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveCartItemAsync(int userId, int productId, string sizeName, bool removeEntireItem)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if(cart == null)
                throw new Exception("Cart not found for this user");

            var cartItem = cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == productId && ci.Size.Equals(sizeName, StringComparison.OrdinalIgnoreCase));

            if(cartItem == null)
                throw new Exception("The specified product with given size is not in the cart.");

            var product = await _context.Products
                .Include(p => p.Sizes)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (removeEntireItem || cartItem.Quantity <= 1)
            {
                cart.CartItems.Remove(cartItem);
                _context.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity -= 1;
                cartItem.Price = cartItem.Price - product.Price;
                cartItem.DiscountedPrice = cartItem.DiscountedPrice - product.DiscountedPrice;
            }

            cart.TotalItems = cart.CartItems.Sum(ci => ci.Quantity);
            //cart.TotalPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.Price);
            cart.TotalPrice = cart.CartItems.Sum(ci => ci.Price);
            //cart.TotalDiscountedPrice = cart.CartItems.Sum(ci => ci.Quantity * ci.DiscountedPrice);
            cart.TotalDiscountedPrice = cart.CartItems.Sum(ci => ci.DiscountedPrice);
            cart.Discount = cart.TotalPrice - cart.TotalDiscountedPrice;

            if(!cart.CartItems.Any())
            {
                cart.TotalItems = 0;
                cart.TotalPrice = 0;
                cart.TotalDiscountedPrice = 0;
                cart.Discount = 0;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<CartItem> GetCartItemByIdAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

            return cartItem;
        }
    }
}
