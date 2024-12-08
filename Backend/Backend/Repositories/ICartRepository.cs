﻿using Backend.Models;

namespace Backend.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task<Cart> GetCartByGuestIdAsync(string guestCartId);
        Task CreateCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task DeleteCartAsync(int cartId);
    }
}