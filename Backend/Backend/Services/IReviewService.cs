using Backend.Models;

namespace Backend.Services
{
    public interface IReviewService
    {
        Task<bool> AddReviewAsync(int userId, int productId, string reviewText);

        Task<List<Review>> GetReviewsByProductIdAsync(int productId);
    }
}
