using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly AppDbContext _context;

        public RatingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddRatingAsync(Rating rating)
        {
            _context.Ratings.Add(rating);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Rating>> GetRatingByProductIdAsync(int productId)
        {
            return await _context.Ratings
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .ToListAsync();
        }

        public async Task<Rating> GetRatingByUserAndProductAsync(int productId, int userId)
        {
            return await _context.Ratings
                .FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);
        }
    }
}
