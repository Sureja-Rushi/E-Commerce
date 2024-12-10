using Backend.Models;

namespace Backend.Repositories
{
    public interface ICartItemRepository
    {
        Task<CartItem> GetCartItemAsync(int cartId, int productId);
        Task<List<CartItem>> GetCartItemsByCartIdAsync(int cartId);
        Task AddCartItemAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task RemoveCartItemAsync(CartItem cartItem);
        Task RemoveAllCartItemsAsync(int cartId);
    }
}
