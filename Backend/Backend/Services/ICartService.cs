using Backend.DTOs;
using Backend.Models;

namespace Backend.Services
{
    public interface ICartService
    {
        //Task AddItemToUserCartAsync(int userId, AddCartItemDto item);
        //Task AddItemToGuestCartAsync(string guestCartId, AddCartItemDto item);
        //Task<CartDto> GetUserCartAsync(int userId);
        //Task<CartDto> GetGuestCartAsync(string guestCartId);
        //Task RemoveItemFromUserCartAsync(int userId, int itemId);
        //Task RemoveItemFromGuestCartAsync(string guestCartId, int itemId);
        //Task MergeGuestCartToUserCartAsync(string guestCartId, int userId);

        int? GetUserIdFromToken();
        Task<CartDto> GetCartAsync();
        Task AddOrUpdateCartItemAsync(AddCartItemDto cartItemDto);
        Task RemoveCartItemAsync(int productId, bool removeAll = false);
    }
}
