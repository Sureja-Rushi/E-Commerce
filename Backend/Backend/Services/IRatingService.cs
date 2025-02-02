using Backend.Models;

namespace Backend.Services
{
    public interface IRatingService
    {
        Task<bool> AddRatingAsync(Rating rating);
        Task<List<Rating>> GetRatingByProductIdAsync(int productId);
    }
}
