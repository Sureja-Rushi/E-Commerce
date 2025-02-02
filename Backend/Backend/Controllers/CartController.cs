using Backend.Helpers;
using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

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

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCart()
        {
            try
            {
                var token = Request.Cookies["AuthToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { message = "Token is not present" });
                }
                var userId = JwtTokenHelper.GetUserFromToken(token).Id;

                var success = await cartService.CreateCartForUserAsync(userId);
                if (!success)
                {
                    return BadRequest(new { message = "Cart already created." });
                }

                return Ok(new { message = "Cart Created Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occured.", details = ex.Message });
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetCart([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            try
            {

                //var token = Request.Cookies["AuthToken"];
                //if (string.IsNullOrEmpty(token))
                //{
                //    return Unauthorized(new { message = "Token is not present" });
                //}

                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    return Unauthorized(new { message = "Authorization token is not provided or invalid." });
                }

                // Extract the JWT token from the Authorization header
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                var userId = JwtTokenHelper.GetUserFromToken(token).Id;

                var cart = await cartService.GetCartForUserAsync(userId);

                if (cart == null)
                {
                    return NotFound(new { message = "Cart not found for the user." });
                }
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occured.", details = ex.Message });
            }
        }

        [HttpPost]
        [Route("add-item")]
        public async Task<IActionResult> AddCartItem([FromBody] AddCartItemRequestDTO request, [FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            try
            {
                //var token = Request.Cookies["AuthToken"];
                //if (string.IsNullOrEmpty(token))
                //{
                //    return Unauthorized(new { message = "Token is not present" });
                //}
                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    return Unauthorized(new { message = "Authorization token is not provided or invalid." });
                }

                // Extract the JWT token from the Authorization header
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                var userId = JwtTokenHelper.GetUserFromToken(token).Id;

                var success = await cartService.AddCartItemAsync(userId, request.ProductId, request.SizeName);

                if (success)
                {
                    return Ok(new { message = "Cart item added successfully" });
                }

                return BadRequest(new { message = "Failed to add cart item." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occured.", details = ex.Message });
            }
        }

        [HttpDelete]
        [Route("removeItem")]
        public async Task<IActionResult> RemoveCartItem([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] AddCartItemRequestDTO request, [FromQuery] bool removeAll = false)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Invalid request." });
                }

                //var token = Request.Cookies["AuthToken"];
                //if (string.IsNullOrEmpty(token))
                //{
                //    return Unauthorized(new { message = "Token is not present" });
                //}
                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    return Unauthorized(new { message = "Authorization token is not provided or invalid." });
                }

                // Extract the JWT token from the Authorization header
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                var userId = JwtTokenHelper.GetUserFromToken(token).Id;

                var success = await cartService.RemoveCartItemAsync(userId, request.ProductId, request.SizeName, removeAll);

                if (success)
                {
                    return Ok(new { message = "Cart item removed successfully" });
                }

                return BadRequest(new { message = "Failed to remove cart item." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occured.", details = ex.Message });
            }
        }

        [HttpGet]
        [Route("item/{id}")]
        public async Task<IActionResult> GetCartItemById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid Cart Item ID." });

                var cartItem = await cartService.GetCartItemByIdAsync(id);

                if (cartItem == null)
                    return NotFound(new { message = "Cart item not found." });

                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

    }
}