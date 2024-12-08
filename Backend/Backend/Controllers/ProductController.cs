using Backend.DTOs;
using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        private User GetLoggedInUser()
        {
            var token = Request.Cookies["AuthToken"]; 
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("User is not logged in.");

            return JwtTokenHelper.GetUserFromToken(token);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var user = GetLoggedInUser();
                var products = await productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProductDetails(int id)
        {
            try
            {
                var user = GetLoggedInUser(); // Check if the user is logged in
                var product = await productService.GetProductDetailsAsync(id);
                return Ok(product);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            try
            {
                var user = GetLoggedInUser(); // Check if the user is logged in
                var products = await productService.GetProductsByCategoryAsync(categoryId);
                return Ok(products);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("sorted")]
        public async Task<IActionResult> GetSortedProducts([FromQuery] string sortBy)
        {
            try
            {
                var user = GetLoggedInUser(); // Check if the user is logged in
                var products = await productService.GetSortedProductsAsync(sortBy);
                return Ok(products);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductDto productDto)
        {
            try
            {
                var user = GetLoggedInUser(); // Check if the user is logged in
                if (user.Role != "Admin")
                    return Forbid("Only admins can add products.");

                await productService.AddProductAsync(productDto);
                return Created("", new { message = "Product added successfully." });
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

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] CreateProductDto productDto)
        {
            try
            {
                var user = GetLoggedInUser(); // Check if the user is logged in
                if (user.Role != "Admin")
                    return Forbid("Only admins can update products.");

                await productService.UpdateProductAsync(id, productDto);
                return Ok(new { message = "Product updated successfully." });
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

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var user = GetLoggedInUser(); // Check if the user is logged in
                if (user.Role != "Admin")
                    return Forbid("Only admins can delete products.");

                await productService.DeleteProductAsync(id);
                return Ok(new { message = "Product deleted successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string search)
        {
            try
            {
                var user = GetLoggedInUser(); // Ensure the user is logged in
                var products = await productService.SearchProductsAsync(search);
                return Ok(products);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("filter")]
        public async Task<IActionResult> SearchProductsWithFilters([FromQuery] string search, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            try
            {
                var user = GetLoggedInUser();
                var products = await productService.SearchProductsWithFiltersAsync(search, minPrice, maxPrice);
                return Ok(products);
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

        [HttpPatch]
        [Route("{id}/update-stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromQuery] int quantityChange)
        {
            try
            {
                var user = GetLoggedInUser(); // Check if the user is logged in
                if (user.Role != "Admin")
                    return Forbid("Only admins can update stock.");

                await productService.UpdateStockAsync(id, quantityChange);
                return Ok(new { message = "Stock updated successfully." });
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
        [Route("{id}/check-stock")]
        public async Task<IActionResult> CheckStock(int id, [FromQuery] int requiredQuantity)
        {
            try
            {
                var user = GetLoggedInUser(); // Check if the user is logged in
                var isAvailable = await productService.CheckStockAvailabilityAsync(id, requiredQuantity);
                return Ok(new { isAvailable });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


    }
}
