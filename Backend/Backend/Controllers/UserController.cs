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

        public UserController(IUserService userService, IAuthService authService)
        {
            this.userService = userService;
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                // Call the service and get the response
                var response = await userService.RegisterAsync(request);

                // Set JWT token in cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsHttps, // Use Secure in HTTPS
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1)
                };

                Response.Cookies.Append("AuthToken", response.Token, cookieOptions);

                // Return the user details as a response
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
