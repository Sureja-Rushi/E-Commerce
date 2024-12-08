using Backend.Models;

namespace Backend.Repositories
{
    public interface IReviewRepository
    {
        Task AddReviewAsync(Review review);
        Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId);
    }
}
