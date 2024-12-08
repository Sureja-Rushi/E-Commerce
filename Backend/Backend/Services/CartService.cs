using Backend.DTOs;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class CartService : ICartService
    {

        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;

        public CartService(ICartRepository cartRepository, ICartItemRepository cartItemRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
        }

        public async Task<Cart> GetCartAsync(int? userId, string guestCartId)
        {
            if (userId.HasValue)
                return await _cartRepository.GetCartByUserIdAsync(userId.Value);

            return await _cartRepository.GetCartByGuestIdAsync(guestCartId);
        }

        public async Task AddOrUpdateCartItemAsync(int? userId, string guestCartId, AddCartItemDto cartItemDto)
        {
            var cart = userId.HasValue
                ? await _cartRepository.GetCartByUserIdAsync(userId.Value)
                : await _cartRepository.GetCartByGuestIdAsync(guestCartId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    GuestCartId = guestCartId,
                    CartItems = new List<CartItem>()
                };

                await _cartRepository.CreateCartAsync(cart);
            }

            var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItemDto.ProductId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += cartItemDto.Quantity;
                await _cartItemRepository.UpdateCartItemAsync(existingCartItem);
            }
            else
            {
                var newCartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = cartItemDto.ProductId,
                    Quantity = cartItemDto.Quantity
                };

                await _cartItemRepository.AddCartItemAsync(newCartItem);
            }
        }

        public async Task RemoveCartItemAsync(int? userId, string guestCartId, int productId)
        {
            var cart = userId.HasValue
                ? await _cartRepository.GetCartByUserIdAsync(userId.Value)
                : await _cartRepository.GetCartByGuestIdAsync(guestCartId);

            if (cart == null) throw new Exception("Cart not found.");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem == null) throw new Exception("Cart item not found.");

            await _cartItemRepository.RemoveCartItemAsync(cartItem.Id);
        }

        public async Task ClearCartAsync(int? userId, string guestCartId)
        {
            var cart = userId.HasValue
                ? await _cartRepository.GetCartByUserIdAsync(userId.Value)
                : await _cartRepository.GetCartByGuestIdAsync(guestCartId);

            if (cart == null) throw new Exception("Cart not found.");

            foreach (var item in cart.CartItems)
                await _cartItemRepository.RemoveCartItemAsync(item.Id);

            await _cartRepository.DeleteCartAsync(cart.Id);
        }

        public async Task MergeGuestCartToUserCartAsync(int userId, string guestCartId)
        {
            var guestCart = await _cartRepository.GetCartByGuestIdAsync(guestCartId);

            if (guestCart == null) return;

            var userCart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (userCart == null)
            {
                guestCart.UserId = userId;
                guestCart.GuestCartId = null;
                await _cartRepository.UpdateCartAsync(guestCart);
            }
            else
            {
                foreach (var guestCartItem in guestCart.CartItems)
                {
                    var existingCartItem = userCart.CartItems.FirstOrDefault(ci => ci.ProductId == guestCartItem.ProductId);

                    if (existingCartItem != null)
                    {
                        existingCartItem.Quantity += guestCartItem.Quantity;
                        await _cartItemRepository.UpdateCartItemAsync(existingCartItem);
                    }
                    else
                    {
                        var newCartItem = new CartItem
                        {
                            CartId = userCart.Id,
                            ProductId = guestCartItem.ProductId,
                            Quantity = guestCartItem.Quantity
                        };
                        await _cartItemRepository.AddCartItemAsync(newCartItem);
                    }
                }

                await _cartRepository.DeleteCartAsync(guestCart.Id);
            }
        }
    }
}
