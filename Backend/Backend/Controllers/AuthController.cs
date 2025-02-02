using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        //private readonly ICartService cartService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
            //this.cartService = cartService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequestDto request)
        {
            try
            {
                var response = await authService.AuthenticateAsync(request);
                SetJwtTokenCookie(response.Token);

                // Check if a guestCartId exists in cookies
                //if (Request.Cookies.TryGetValue("guestCartId", out var guestCartId) && !string.IsNullOrEmpty(guestCartId))
                //{
                //    // Convert the guest cart to the logged-in user's cart
                //    await cartService.MergeGuestCartToUserCartAsync(guestCartId, response.User.Id);

                //    // Remove the guestCartId cookie after conversion
                //    Response.Cookies.Delete("guestCartId");
                //}

                //string guestCartId = HttpContext.Session.GetString("GuestCartId");

                //if (!string.IsNullOrEmpty(guestCartId))
                //{
                //    // Update the guest cart to the logged-in user
                //    await cartService.UpdateCartToUserIdAsync(response.User.Id, guestCartId);
                //}

                return Ok(new
                {
                    message = "Login Successful!",
                    token = response.Token
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid email or password.");
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
