using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IAuthService authService;
        private readonly ICartService cartService;

        public UserController(IUserService userService, IAuthService authService, ICartService cartService)
        {
            this.userService = userService;
            this.authService = authService;
            this.cartService = cartService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                var response = await userService.RegisterAsync(request);

                // Set JWT token in cookies
                SetJwtTokenCookie(response.Token);

                // Check if a guestCartId exists in cookies
                //if (Request.Cookies.TryGetValue("guestCartId", out var guestCartId) && !string.IsNullOrEmpty(guestCartId))
                //{
                //    // Convert the guest cart to the registered user's cart
                //    await cartService.MergeGuestCartToUserCartAsync(guestCartId, response.User.Id);

                //    // Remove the guestCartId cookie after conversion
                //    Response.Cookies.Delete("guestCartId");
                //}

                return CreatedAtAction(nameof(Register), new { id = response.User.Id }, new
                {
                    message = $"Welcome {response.User.FullName}, registration successful!",
                    token = response.Token
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        private void SetJwtTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            };

            Response.Cookies.Append("AuthToken", token, cookieOptions);
        }
    }
}
