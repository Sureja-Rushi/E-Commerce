using Backend.DTOs;
using Backend.Helpers;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var cart = await _cartService.GetCartAsync();
                if (cart == null)
                    return NotFound(new { message = "Cart not found." });

                return Ok(cart);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "User is not logged in." });
            }
        }


        [HttpPost("items")]
        public async Task<IActionResult> AddOrUpdateCartItem([FromBody] AddCartItemDto cartItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _cartService.AddOrUpdateCartItemAsync(cartItemDto);
                return Ok(new { message = "Item added/updated in the cart successfully" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpDelete("items/{productId}")]
        public async Task<IActionResult> RemoveCartItem(int productId, [FromQuery] bool removeAll = false)
        {
            try
            {
                await _cartService.RemoveCartItemAsync(productId, removeAll);
                return Ok(new { message = removeAll ? "Item removed from cart" : "Item quantity decreased" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
