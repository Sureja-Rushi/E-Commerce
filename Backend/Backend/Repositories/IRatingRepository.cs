using Backend.Models;

namespace Backend.Repositories
{
    public interface IRatingRepository
    {
        Task<bool> AddRatingAsync(Rating rating);
        Task<Rating> GetRatingByUserAndProductAsync(int productId, int userId);
        Task<List<Rating>> GetRatingByProductIdAsync(int productId);
    }
}
