using Backend.DTOs;
using Backend.Models;

namespace Backend.Services
{
    public interface ICartService
    {
        Task<bool> CreateCartForUserAsync(int userId);
        Task<Cart> GetCartForUserAsync(int userId);
        Task<bool> AddCartItemAsync(int userId, int productId, string sizeName);
        Task<bool> RemoveCartItemAsync(int userId, int productId, string sizeName, bool removeEntireItem);
        Task<CartItemDto> GetCartItemByIdAsync(int cartItemId);
    }
}
