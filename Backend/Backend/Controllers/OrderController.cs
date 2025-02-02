using Backend.Helpers;
using Backend.Models;
using Backend.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Climate;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrderFromCart([FromBody] CreateOrderRequestDTO request, [FromHeader(Name = "Authorization")] string authorizationHeader)
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

                if (userId <= 0)
                    return Unauthorized(new { message = "User not authorized." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Address newAddress = null;
                if (request.NewAddress != null)
                {
                    newAddress = new Address
                    {
                        Street = request.NewAddress.Street,
                        City = request.NewAddress.City,
                        State = request.NewAddress.State,
                        ZipCode = request.NewAddress.ZipCode,
                        FirstName = request.NewAddress.FirstName,
                        LastName = request.NewAddress.LastName,
                        ContactNumber = request.NewAddress.ContactNumber
                    };
                }

                var result = await orderService.CreateOrderFromCartAsync(userId, request.ExistingAddressId, newAddress);
                if (!result.Success)
                {
                    return BadRequest(new { message = result.Message });
                }

                var createdOrder = await orderService.GetOrderByIdAsync(result.OrderId);

                if (createdOrder == null)
                {
                    return StatusCode(500, new { message = "Order created but could not fetch details." });
                }
                return Ok(new
                {
                    message = "Order created successfully.",
                    order = new
                    {
                        createdOrder.Id,
                        createdOrder.UserId,
                        createdOrder.TotalPrice,
                        createdOrder.TotalDiscountedPrice,
                        createdOrder.Discount,
                        createdOrder.OrderStatus,
                        createdOrder.OrderDate,
                        createdOrder.DeliveryDate,
                        ShippingAddress = new
                        {
                            createdOrder.ShippingAddress.FirstName,
                            createdOrder.ShippingAddress.LastName,
                            createdOrder.ShippingAddress.ContactNumber,
                            createdOrder.ShippingAddress.Street,
                            createdOrder.ShippingAddress.City,
                            createdOrder.ShippingAddress.State,
                            createdOrder.ShippingAddress.ZipCode
                        },
                        OrderItems = createdOrder.OrderItems.Select(oi => new
                        {
                            oi.Id,
                            Product = new
                            {
                                oi.Product.Id,
                                oi.Product.Title,
                                oi.Product.Price,
                                oi.Product.Brand
                            },
                            oi.Quantity,
                            oi.Price,
                            oi.DiscountedPrice
                        })
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpPut]
        [Route("{orderId}/status")]
        public async Task<IActionResult> ChangeOrderStatus(int orderId, [FromQuery] string status)
        {
            try
            {
                if (string.IsNullOrEmpty(status))
                    return BadRequest(new { message = "Status is required." });

                var result = await orderService.ChangeOrderStatusAsync(orderId, status);

                return result
                    ? Ok(new { message = $"Order status updated to {status}" })
                    : NotFound(new { message = "Order not found" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet]
        [Route("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            try
            {
                var order = await orderService.GetOrderByIdAsync(orderId);

                return Ok(new
                {
                    order.Id,
                    order.UserId,
                    User = new
                    {
                        order.User.Id,
                        order.User.FirstName,
                        order.User.LastName,
                        order.User.Email
                    },
                    order.TotalPrice,
                    order.TotalDiscountedPrice,
                    order.Discount,
                    order.OrderStatus,
                    order.OrderDate,
                    order.DeliveryDate,
                    ShippingAddress = new
                    {
                        order.ShippingAddress.Street,
                        order.ShippingAddress.City,
                        order.ShippingAddress.State,
                        order.ShippingAddress.ZipCode,
                        order.ShippingAddress.FirstName,
                        order.ShippingAddress.LastName,
                        order.ShippingAddress.ContactNumber
                    },
                    OrderItems = order.OrderItems.Select(oi => new
                    {
                        oi.Id,
                        Product = new
                        {
                            oi.Product.Id,
                            oi.Product.Title,
                            oi.Product.Price,
                            oi.Product.Brand,
                            oi.Product.ImageUrl,
                            oi.Product.DiscountedPrice,
                            oi.Product.Color,
                        },
                        oi.Size,
                        oi.Quantity,
                        oi.Price,
                        oi.DiscountedPrice
                    })
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet]
        [Route("history/{userId}")]
        public async Task<IActionResult> GetUserOrderHistory(int userId)
        {
            try
            {
                var orders = await orderService.GetUserOrderHistoryAsync(userId);

                return Ok(orders.Select(order => new
                {
                    order.Id,
                    order.TotalPrice,
                    order.TotalDiscountedPrice,
                    order.Discount,
                    order.OrderStatus,
                    order.OrderDate,
                    order.DeliveryDate,
                    ShippingAddress = new
                    {
                        order.ShippingAddress.Street,
                        order.ShippingAddress.City,
                        order.ShippingAddress.State,
                        order.ShippingAddress.ZipCode
                    },
                    OrderItems = order.OrderItems.Select(oi => new
                    {
                        oi.Id,
                        Product = new
                        {
                            oi.Product.Id,
                            oi.Product.Title,
                            oi.Product.Price,
                            oi.Product.Brand
                        },
                        oi.Quantity,
                        oi.Price,
                        oi.DiscountedPrice
                    })
                }));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await orderService.GetAllOrdersAsync();

                return Ok(orders.Select(order => new
                {
                    order.Id,
                    order.TotalPrice,
                    order.TotalDiscountedPrice,
                    order.Discount,
                    order.OrderStatus,
                    order.OrderDate,
                    order.DeliveryDate,
                    User = new
                    {
                        order.User.Id,
                        order.User.FirstName,
                        order.User.LastName,
                        order.User.Email
                    },
                    ShippingAddress = new
                    {
                        order.ShippingAddress.Street,
                        order.ShippingAddress.City,
                        order.ShippingAddress.State,
                        order.ShippingAddress.ZipCode
                    },
                    OrderItems = order.OrderItems.Select(oi => new
                    {
                        oi.Id,
                        Product = new
                        {
                            oi.Product.Id,
                            oi.Product.Title,
                            oi.Product.Price,
                            oi.Product.Brand,
                            oi.Product.ImageUrl
                        },
                        oi.Quantity,
                        oi.Price,
                        oi.DiscountedPrice
                    })
                }));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpDelete]
        [Route("{orderId}")]
        public async Task<IActionResult> DeleteOrderById(int orderId)
        {
            try
            {
                await orderService.DeleteOrderByIdAsync(orderId);
                return Ok(new { message = "Order deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }
    }
}
