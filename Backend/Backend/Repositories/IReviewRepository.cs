using Backend.Models;

namespace Backend.Repositories
{
    public interface IReviewRepository
    {
        Task<bool> AddReviewAsync(Review review);

        Task<List<Review>> GetReviewsByProductIdAsync(int productId);
    }
}
