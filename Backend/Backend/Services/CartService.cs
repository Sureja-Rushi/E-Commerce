
using Backend.DTOs;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }

        public async Task<bool> AddCartItemAsync(int userId, int productId, string sizeName)
        {
            if(userId <= 0)
            {
                throw new ArgumentException("Invalid user Id");
            }
            if (productId <= 0)
            {
                throw new ArgumentException("Invalid product Id.");
            }
            if(string.IsNullOrWhiteSpace(sizeName))
            {
                throw new ArgumentException("Invalid size name.");
            }

            return await cartRepository.AddCartItemAsync(userId, productId, sizeName);
        }

        public async Task<bool> CreateCartForUserAsync(int userId)
        {
            return await cartRepository.CreateCartAsync(userId);
        }

        public async Task<Cart> GetCartForUserAsync(int userId)
        {
            return await cartRepository.GetCartByUserIdAsync(userId);
        }

        public async Task<CartItemDto> GetCartItemByIdAsync(int cartItemId)
        {
            if (cartItemId <= 0)
                throw new ArgumentException("Invalid Cart Item ID.");

            var cartItem = await cartRepository.GetCartItemByIdAsync(cartItemId);

            if (cartItem == null)
                return null;

            // Map the cart item to a DTO
            return new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product.Title,
                Size = cartItem.Size,
                Quantity = cartItem.Quantity,
                Price = cartItem.Price,
                DiscountedPrice = cartItem.DiscountedPrice,
                UserId = cartItem.UserId
            };
        }

        public async Task<bool> RemoveCartItemAsync(int userId, int productId, string sizeName, bool removeEntireItem)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user Id");
            }
            if (productId <= 0)
            {
                throw new ArgumentException("Invalid product Id.");
            }
            if (string.IsNullOrWhiteSpace(sizeName))
            {
                throw new ArgumentException("Invalid size name.");
            }

            return await cartRepository.RemoveCartItemAsync(userId, productId, sizeName, removeEntireItem);
        }
    }
}
