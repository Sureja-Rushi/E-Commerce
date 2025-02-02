using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository reviewRepository;
        private readonly IUserRepository userRepository;
        private readonly IProductRepository productRepository;

        public ReviewService(IReviewRepository reviewRepository, IUserRepository userRepository, IProductRepository productRepository)
        {
            this.reviewRepository = reviewRepository;
            this.userRepository = userRepository;
            this.productRepository = productRepository;
        }

        public async Task<bool> AddReviewAsync(int userId, int productId, string reviewText)
        {
            var review = new Review
            {
                UserId = userId,
                User = await userRepository.GetUserByIdAsync(userId),
                ProductId = productId,
                Product = await productRepository.GetProductByIdAsync(productId),
                ReviewText = reviewText,
                CreatedAt = DateTime.UtcNow
            };

            return await reviewRepository.AddReviewAsync(review);
        }

        public async Task<List<Review>> GetReviewsByProductIdAsync(int productId)
        {
            return await reviewRepository.GetReviewsByProductIdAsync(productId);
        }
    }
}
