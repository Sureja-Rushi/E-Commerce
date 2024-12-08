using Backend.DTOs;
using Backend.Helpers;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/review")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpPost]
        [Route("{productId}")]
        public async Task<IActionResult> AddReview(int productId, [FromBody] AddReviewDto reviewDto)
        {
            try
            {
                var token = Request.Cookies["AuthToken"];
                if (string.IsNullOrEmpty(token))
                    return Unauthorized(new { message = "Authorization token is missing." });

                var user = JwtTokenHelper.GetUserFromToken(token);

                if (user.Role != "User")
                    return Forbid("Only users can add reviews.");

                await reviewService.AddReviewAsync(productId, reviewDto, user.Id);
                return Created("", new { message = "Review added successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> GetReviews(int productId)
        {
            try
            {
                var reviews = await reviewService.GetReviewsByProductIdAsync(productId);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
