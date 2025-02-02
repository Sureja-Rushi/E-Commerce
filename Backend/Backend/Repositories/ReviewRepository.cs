using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;
        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddReviewAsync(Review review)
        {
            if(review == null) throw new ArgumentNullException(nameof(review));

            var productExists = await _context.Products.AnyAsync(p => p.Id == review.ProductId);
            if(!productExists)
            {
                return false;
            }

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Review>> GetReviewsByProductIdAsync(int productId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}
