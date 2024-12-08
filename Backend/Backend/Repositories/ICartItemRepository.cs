using Backend.Models;

namespace Backend.Repositories
{
    public interface ICartItemRepository
    {
        Task<CartItem> GetCartItemAsync(int cartId, int productId);
        Task AddCartItemAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task RemoveCartItemAsync(int cartItemId);
        Task<List<CartItem>> GetCartItemsByCartIdAsync(int cartId);
    }
}
