using Backend.DTOs;
using Backend.Models;

namespace Backend.Services
{
    public interface ICartService
    {
        Task<Cart> GetCartAsync(int? userId, string guestCartId);
        Task AddOrUpdateCartItemAsync(int? userId, string guestCartId, AddCartItemDto cartItemDto);
        Task RemoveCartItemAsync(int? userId, string guestCartId, int productId);
        Task ClearCartAsync(int? userId, string guestCartId);
        Task MergeGuestCartToUserCartAsync(int userId, string guestCartId);
    }
}
