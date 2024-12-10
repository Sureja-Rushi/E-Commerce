using Backend.Helpers;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            // Get userId from JWT token
            var userId = GetUserIdFromToken();
            if (userId == null) return Unauthorized("User not authenticated.");

            try
            {
                var order = await _orderService.CreateOrderAsync(userId.Value, request.ShippingAddress);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var authToken = Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(authToken))
            {
                return Unauthorized("User is not logged in.");
            }

            var user = JwtTokenHelper.GetUserFromToken(authToken);
            if (user == null)
            {
                return Unauthorized("Invalid token.");
            }

            // Call the service to get order details
            var orderDetails = await _orderService.GetOrderDetailsAsync(orderId, user.Id);

            return Ok(orderDetails);
        }

        [HttpPost("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto updateOrderStatusDto)
        {
            // Get userId from JWT token
            var adminUserId = GetUserIdFromToken();
            if (adminUserId == null) return Unauthorized("User not authenticated.");

            // Validate the input
            if (string.IsNullOrWhiteSpace(updateOrderStatusDto.Status))
                return BadRequest("Status is required.");

            try
            {
                await _orderService.UpdateOrderStatusAsync(orderId, updateOrderStatusDto.Status, adminUserId.Value);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserOrders(int userId)
        {
            // Get userId from JWT token (optional, if the route is dynamic)
            try
            {
                var orders = await _orderService.GetUserOrdersAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status))
                    return BadRequest("Status is required.");

                var orders = await _orderService.GetOrdersByStatusAsync(status);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private int? GetUserIdFromToken()
        {
            var token = Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token)) return null;

            var user = JwtTokenHelper.GetUserFromToken(token);
            return user?.Id;
        }
    }

    public class CreateOrderRequest
    {
        public string ShippingAddress { get; set; }
    }

    public class UpdateOrderStatusDto
    {
        public string Status { get; set; }
    }
}
