using Backend.Models;

namespace Backend.Repositories
{
    public interface ICartRepository
    {
        Task<bool> CreateCartAsync(int userId);
        Task<bool> CartExistsAsync(int userId);
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task<bool> AddCartItemAsync(int userId, int productId, string sizeName);
        Task<bool> RemoveCartItemAsync(int userId, int productId, string sizeName, bool removeEntireItem);
        Task<CartItem> GetCartItemByIdAsync(int cartItemId);
    }
}
