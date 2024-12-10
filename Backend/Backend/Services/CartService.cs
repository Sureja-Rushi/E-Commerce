using Backend.DTOs;
using Backend.Helpers;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(ICartRepository cartRepository, ICartItemRepository cartItemRepository, IHttpContextAccessor httpContextAccessor)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetUserIdFromToken()
        {
            var authToken = _httpContextAccessor.HttpContext.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(authToken)) return null;

            var user = JwtTokenHelper.GetUserFromToken(authToken);
            return user?.Id;
        }

        public async Task<CartDto> GetCartAsync()
        {
            int? userId = GetUserIdFromToken();
            if (userId == null)
                throw new UnauthorizedAccessException("User is not logged in.");

            var cart = await _cartRepository.GetCartByUserIdAsync(userId.Value);
            if (cart == null)
                return null;

            return TransformToCartDto(cart);
        }

        public async Task AddOrUpdateCartItemAsync(AddCartItemDto cartItemDto)
        {
            int? userId = GetUserIdFromToken();
            if (!userId.HasValue)
                throw new UnauthorizedAccessException("User is not logged in.");

            // Fetch the user's cart, create a new one if it doesn't exist
            var cart = await _cartRepository.GetCartByUserIdAsync(userId.Value);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId.Value,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CartItems = new List<CartItem>()
                };
                await _cartRepository.AddCartAsync(cart);
            }

            // Check if the cart already has the item
            var cartItem = await _cartItemRepository.GetCartItemAsync(cart.Id, cartItemDto.ProductId);
            if (cartItem == null)
            {
                // Add a new cart item with quantity initialized to 1
                cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = cartItemDto.ProductId,
                    Quantity = 1, // Always start with 1
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _cartItemRepository.AddCartItemAsync(cartItem);
            }
            else
            {
                // Increment the quantity by 1
                cartItem.Quantity += 1;
                cartItem.UpdatedAt = DateTime.UtcNow;
                await _cartItemRepository.UpdateCartItemAsync(cartItem);
            }

            // Update the cart's updated timestamp
            cart.UpdatedAt = DateTime.UtcNow;
            await _cartRepository.UpdateCartAsync(cart);
        }


        public async Task RemoveCartItemAsync(int productId, bool removeAll = false)
        {
            int? userId = GetUserIdFromToken();
            if (!userId.HasValue)
                throw new UnauthorizedAccessException("User is not logged in.");

            var cart = await _cartRepository.GetCartByUserIdAsync(userId.Value);
            if (cart == null) return;

            var cartItem = await _cartItemRepository.GetCartItemAsync(cart.Id, productId);
            if (cartItem == null) return;

            if (removeAll || cartItem.Quantity <= 1)
            {
                await _cartItemRepository.RemoveCartItemAsync(cartItem);
            }
            else
            {
                cartItem.Quantity -= 1;
                cartItem.UpdatedAt = DateTime.UtcNow;
                await _cartItemRepository.UpdateCartItemAsync(cartItem);
            }

            var remainingItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cart.Id);
            if (!remainingItems.Any())
            {
                await _cartRepository.DeleteCartAsync(cart.Id);
            }
        }

        private CartDto TransformToCartDto(Cart cart)
        {
            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                CartItems = cart.CartItems?.Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name ?? "Unknown Product",
                    ProductPrice = ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity,
                    Subtotal = ci.Quantity * (ci.Product?.Price ?? 0)
                }).ToList() ?? new List<CartItemDto>(),
                TotalPrice = cart.CartItems?.Sum(ci => ci.Quantity * (ci.Product?.Price ?? 0)) ?? 0
            };
        }
    }
}
