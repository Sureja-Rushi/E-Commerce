using Backend.DTOs;

namespace Backend.Services
{
    public interface IReviewService
    {
        Task AddReviewAsync(int productId, AddReviewDto reviewDto, int userId);
        Task<IEnumerable<ReviewDto>> GetReviewsByProductIdAsync(int productId);
    }
}
