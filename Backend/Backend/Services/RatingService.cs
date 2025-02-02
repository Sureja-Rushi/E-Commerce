using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            this.ratingRepository = ratingRepository;
        }

        public async Task<bool> AddRatingAsync(Rating rating)
        {
            var existingRating = await ratingRepository.GetRatingByUserAndProductAsync(rating.ProductId, rating.UserId);
            if (existingRating != null)
            {
                throw new InvalidOperationException("User has already rated to this product");
            }
            return await ratingRepository.AddRatingAsync(rating);
        }

        public async Task<List<Rating>> GetRatingByProductIdAsync(int productId)
        {
            return await ratingRepository.GetRatingByProductIdAsync(productId);
        }
    }
}
