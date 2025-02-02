using Backend.Services;
using Backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Stripe;
using Backend.Helpers;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService reviewService;
        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddReview([FromBody] AddReviewRequestDTO reviewDto)
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
                var success = await reviewService.AddReviewAsync(
                    userId,
                    reviewDto.ProductId,
                    reviewDto.ReviewText);

                return success ? Ok(new { message = "Review added Successfully." }) : BadRequest(new { message = "Failed to add review." });
            }
            catch (Exception ex)
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
                var reviews = await reviewService.GetReviewsByProductIdAsync(productId);
                if(reviews == null || !reviews.Any())
                {
                    return NotFound(new { message = "No reviews found for this product." });
                }
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error Occured", details = ex.Message });
            }
        }
    }
}
