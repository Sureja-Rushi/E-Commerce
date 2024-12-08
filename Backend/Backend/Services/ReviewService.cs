using Backend.DTOs;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository reviewRepository;
        private readonly IProductRepository productRepository;

        public ReviewService(IReviewRepository reviewRepository, IProductRepository productRepository)
        {
            this.reviewRepository = reviewRepository;
            this.productRepository = productRepository;
        }

        public async Task AddReviewAsync(int productId, AddReviewDto reviewDto, int userId)
        {
            // Validate product existence
            var product = await productRepository.GetProductByIdAsync(productId);
            if (product == null)
                throw new Exception("Product not found.");

            // Create a new review
            var review = new Review
            {
                Comment = reviewDto.Comment,
                Rating = reviewDto.Rating,
                ProductId = productId,
                UserId = userId
            };

            // Save the review
            await reviewRepository.AddReviewAsync(review);
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByProductIdAsync(int productId)
        {
            // Get all reviews for a specific product
            var reviews = await reviewRepository.GetReviewsByProductIdAsync(productId);
            return reviews.Select(r => new ReviewDto
            {
                Comment = r.Comment,
                Rating = r.Rating,
                UserName = r.User.FullName
            }).ToList();
        }

    }
}
