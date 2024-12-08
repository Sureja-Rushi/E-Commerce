using Backend.DTOs;
using Backend.Helpers;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        private (int? userId, string guestCartId) GetUserIdOrGuestCartId()
        {
            string token = Request.Cookies["AuthToken"];
            int? userId = null;
            string guestCartId = null;

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var user = JwtTokenHelper.GetUserFromToken(token);
                    userId = user.Id;
                }
                catch
                {
                    throw new UnauthorizedAccessException("Invalid or expired JWT token.");
                }
            }

            if (userId == null)
            {
                if (!Request.Cookies.TryGetValue("guestCartId", out guestCartId))
                {
                    guestCartId = Guid.NewGuid().ToString();
                    Response.Cookies.Append("guestCartId", guestCartId, new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddDays(7)
                    });
                }
            }

            return (userId, guestCartId);
        }


        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var (userId, guestCartId) = GetUserIdOrGuestCartId();
            var cart = await cartService.GetCartAsync(userId, guestCartId);
            if (cart == null) return NotFound("Cart not found.");
            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddOrUpdateCartItem([FromBody] AddCartItemDto cartItemDto)
        {
            var (userId, guestCartId) = GetUserIdOrGuestCartId();
            await cartService.AddOrUpdateCartItemAsync(userId, guestCartId, cartItemDto);
            return Ok("Cart item added or updated successfully.");
        }

        [HttpDelete("items/{productId}")]
        public async Task<IActionResult> RemoveCartItem(int productId)
        {
            var (userId, guestCartId) = GetUserIdOrGuestCartId();
            await cartService.RemoveCartItemAsync(userId, guestCartId, productId);
            return Ok("Cart item removed successfully.");
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var (userId, guestCartId) = GetUserIdOrGuestCartId();
            await cartService.ClearCartAsync(userId, guestCartId);
            return Ok("Cart cleared successfully.");
        }

        [HttpPost("merge")]
        public async Task<IActionResult> MergeGuestCartToUserCart()
        {
            string token = Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token)) return Unauthorized("User is not logged in.");

            var user = JwtTokenHelper.GetUserFromToken(token);
            if (!Request.Cookies.TryGetValue("guestCartId", out var guestCartId))
                return BadRequest("Guest cart ID not found.");

            await cartService.MergeGuestCartToUserCartAsync(user.Id, guestCartId);

            Response.Cookies.Delete("guestCartId");

            return Ok("Guest cart merged into user cart successfully.");
        }
    }
}
