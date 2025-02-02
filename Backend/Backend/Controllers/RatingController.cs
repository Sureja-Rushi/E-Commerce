using Backend.DTOs;
using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Backend.Repositories;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/rating")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService ratingService;
        private readonly IProductRepository productRepository;
        private readonly IUserRepository userRepository;

        public RatingController(IRatingService ratingService, IProductRepository productRepository, IUserRepository userRepository)
        {
            this.ratingService = ratingService;
            this.productRepository = productRepository;
            this.userRepository = userRepository;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddRating([FromBody] AddRatingRequestDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var token = Request.Cookies["AuthToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { message = "Token is not present" });
                }
                var userId = JwtTokenHelper.GetUserFromToken(token).Id;
                var rating = new Rating
                {
                    ProductId = request.ProductId,
                    UserId = userId,
                    RatingNumber = request.RatingNumber,
                    Product = await productRepository.GetProductByIdAsync(request.ProductId),
                    User = await userRepository.GetUserByIdAsync(userId)
                };

                var success = await ratingService.AddRatingAsync(rating);

                return success ? Ok(new {message = "Rating added Successfully." }) : BadRequest(new { message = "Failed to add rating." });
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "An error Occured", details = ex.Message });
            }
        }

        [HttpGet]
        [Route("product/{productId}")]
        public async Task<IActionResult> GetReviewsByProductId(int productId)
        {
            try
            {
                var ratings = await ratingService.GetRatingByProductIdAsync(productId);
                if (ratings == null || !ratings.Any())
                {
                    return NotFound(new { message = "No ratings found for this product." });
                }
                return Ok(ratings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error Occured", details = ex.Message });
            }
        }
    }
}
