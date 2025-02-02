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

        //private User GetLoggedInUser()
        //{
        //    var token = Request.Cookies["AuthToken"]; 
        //    if (string.IsNullOrEmpty(token))
        //        throw new UnauthorizedAccessException("User is not logged in.");

        //    return JwtTokenHelper.GetUserFromToken(token);
        //}

        //[HttpGet]
        //[Route("")]
        //public async Task<IActionResult> GetAllProducts()
        //{
        //    try
        //    {
        //        var user = GetLoggedInUser();
        //        var products = await productService.GetAllProductsAsync();
        //        return Ok(products);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(new { message = ex.Message });
        //    }
        //}

        //[HttpGet]
        //[Route("{id}")]
        //public async Task<IActionResult> GetProductDetails(int id)
        //{
        //    try
        //    {
        //        var user = GetLoggedInUser(); // Check if the user is logged in
        //        var product = await productService.GetProductDetailsAsync(id);
        //        return Ok(product);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(new { message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return NotFound(new { message = ex.Message });
        //    }
        //}

        //[HttpGet]
        //[Route("category/{categoryId}")]
        //public async Task<IActionResult> GetProductsByCategory(int categoryId)
        //{
        //    try
        //    {
        //        var user = GetLoggedInUser(); // Check if the user is logged in
        //        var products = await productService.GetProductsByCategoryAsync(categoryId);
        //        return Ok(products);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(new { message = ex.Message });
        //    }
        //}

        //[HttpGet]
        //[Route("sorted")]
        //public async Task<IActionResult> GetSortedProducts([FromQuery] string sortBy)
        //{
        //    try
        //    {
        //        var user = GetLoggedInUser(); // Check if the user is logged in
        //        var products = await productService.GetSortedProductsAsync(sortBy);
        //        return Ok(products);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(new { message = ex.Message });
        //    }
        //}

        //[HttpPost]
        //[Route("")]
        //public async Task<IActionResult> AddProduct([FromBody] CreateProductDto productDto)
        //{
        //    try
        //    {
        //        var user = GetLoggedInUser(); // Check if the user is logged in
        //        if (user.Role != "Admin")
        //            return Forbid("Only admins can add products.");

        //        await productService.AddProductAsync(productDto);
        //        return Created("", new { message = "Product added successfully." });
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(new { message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        //[HttpPut]
        //[Route("{id}")]
        //public async Task<IActionResult> UpdateProduct(int id, [FromBody] CreateProductDto productDto)
        //{
        //    try
        //    {
        //        var user = GetLoggedInUser(); // Check if the user is logged in
        //        if (user.Role != "Admin")
        //            return Forbid("Only admins can update products.");

        //        await productService.UpdateProductAsync(id, productDto);
        //        return Ok(new { message = "Product updated successfully." });
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(new { message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        //[HttpDelete]
        //[Route("{id}")]
        //public async Task<IActionResult> DeleteProduct(int id)
        //{
        //    try
        //    {
        //        var user = GetLoggedInUser(); // Check if the user is logged in
        //        if (user.Role != "Admin")
        //            return Forbid("Only admins can delete products.");

        //        await productService.DeleteProductAsync(id);
        //        return Ok(new { message = "Product deleted successfully." });
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(new { message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return NotFound(new { message = ex.Message });
        //    }
        //}

        //[HttpGet]
        //[Route("search")]
        //public async Task<IActionResult> SearchProducts([FromQuery] string search)
        //{
        //    try
        //    {
        //        var user = GetLoggedInUser(); // Ensure the user is logged in
        //        var products = await productService.SearchProductsAsync(search);
        //        return Ok(products);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(new { message = ex.Message });
        //    }
        //}

        //[HttpGet]
        //[Route("filter")]
        //public async Task<IActionResult> SearchProductsWithFilters([FromQuery] string search, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        //{
        //    try
        //    {
        //        var user = GetLoggedInUser();
        //        var products = await productService.SearchProductsWithFiltersAsync(search, minPrice, maxPrice);
        //        return Ok(products);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(new { message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        //[HttpPatch]
        //[Route("{id}/update-stock")]
        //public async Task<IActionResult> UpdateStock(int id, [FromQuery] int quantityChange)
        //{
        //    try
        //    {
        //        var user = GetLoggedInUser(); // Check if the user is logged in
        //        if (user.Role != "Admin")
        //            return Forbid("Only admins can update stock.");

        //        await productService.UpdateStockAsync(id, quantityChange);
        //        return Ok(new { message = "Stock updated successfully." });
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(new { message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        //[HttpGet]
        //[Route("{id}/check-stock")]
        //public async Task<IActionResult> CheckStock(int id, [FromQuery] int requiredQuantity)
        //{
        //    try
        //    {
        //        var user = GetLoggedInUser(); // Check if the user is logged in
        //        var isAvailable = await productService.CheckStockAvailabilityAsync(id, requiredQuantity);
        //        return Ok(new { isAvailable });
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        return Unauthorized(new { message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return NotFound(new { message = ex.Message });
        //    }
        //}

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductRequestDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("Validation failed");
                    return BadRequest(ModelState);
                }

                // Calculate total size quantity if sizes are provided
                var totalSizeQuantity = request.Sizes?.Sum(size => size.Quantity) ?? 0;

                // Validate product quantity
                if (request.Sizes != null && totalSizeQuantity > request.Quantity)
                {
                    return BadRequest(new
                    {
                        message = "Product quantity must be greater than or equal to the sum of all size quantities."
                    });
                }

                var product = new Product
                {
                    Title = request.Title,
                    Description = request.Description,
                    Price = request.Price,
                    DiscountedPrice = request.DiscountedPrice,
                    DiscountPercent = request.DiscountPercent,
                    Quantity = request.Quantity,
                    Brand = request.Brand,
                    Color = request.Color,
                    ImageUrl = request.ImageUrl,
                    CreatedAt = DateTime.UtcNow
                };

                // Map sizes if provided
                var productSizes = request.Sizes?.Select(size => new ProductSize
                {
                    SizeName = size.SizeName,
                    Quantity = size.Quantity
                }).ToList();

                var success = await productService.AddProductAsync(
                    product,
                    request.CategoryName,
                    request.ParentCategoryName,
                    request.GrandParentCategoryName,
                    productSizes);

                return success
                    ? Ok(new { message = "Product added successfully." })
                    : BadRequest(new { message = "Failed to add product." });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var success = await productService.DeleteProductAsync(id);

                return success
                    ? Ok(new { message = "Product deleted successfully." })
                    : NotFound(new { message = "Product not found." });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequestDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var success = await productService.UpdateProductAsync(
                    id,
                    request.Title,
                    request.Description,
                    request.Price,
                    request.DiscountedPrice,
                    request.DiscountPercent,
                    request.Quantity,
                    request.Brand,
                    request.Color,
                    request.ImageUrl,
                    request.CategoryName,
                    request.ParentCategoryName,
                    request.GrandParentCategoryName,
                    request.Sizes);

                return success
                    ? Ok(new { message = "Product updated successfully." })
                    : NotFound(new { message = "Product not found." });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await productService.GetProductByIdAsync(id);
                return product != null
                    ? Ok(product)
                    : NotFound(new { message = "Product not found." });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        //[HttpGet]
        //[Route("")]
        //public async Task<IActionResult> GetAllProducts()
        //{
        //    try
        //    {
        //        var products = await productService.GetAllProductsAsync();
        //        return Ok(products);
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        return NotFound(new { message = "No products found." });
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception: " + ex.Message);
        //        return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
        //    }
        //}

        //[HttpGet]
        //[Route("filter")]
        //public async Task<IActionResult> GetProducts(
        //    [FromQuery] string? color = null,
        //    [FromQuery] string? size = null,
        //    [FromQuery] decimal? minPrice = null,
        //    [FromQuery] decimal? maxPrice = null,
        //    [FromQuery] decimal? minDiscount = null,
        //    [FromQuery] string? category = null,
        //    [FromQuery] bool? stock = null,
        //    [FromQuery] string? sort = null,
        //    [FromQuery] int pageNumber = 1,
        //    [FromQuery] int pageSize = 10)
        //{
        //    try
        //    {
        //        var products = await productService.GetProductsAsync(
        //            color,
        //            size,
        //            minPrice,
        //            maxPrice,
        //            minDiscount,
        //            category,
        //            stock,
        //            sort,
        //            pageNumber,
        //            pageSize);
        //        return Ok(products);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
        //    }
        //}

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetProducts(
    [FromQuery] string? color = null,
    [FromQuery] string? size = null,
    [FromQuery] decimal? minPrice = null,
    [FromQuery] decimal? maxPrice = null,
    [FromQuery] decimal? minDiscount = null,
    [FromQuery] string? category = null,
    [FromQuery] bool? stock = null,
    [FromQuery] string? sort = null,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            try
            {
                // If no parameters are provided, return all products without filtering
                if (string.IsNullOrEmpty(color) &&
                    string.IsNullOrEmpty(size) &&
                    !minPrice.HasValue &&
                    !maxPrice.HasValue &&
                    !minDiscount.HasValue &&
                    string.IsNullOrEmpty(category) &&
                    !stock.HasValue &&
                    string.IsNullOrEmpty(sort))
                {
                    var products = await productService.GetAllProductsAsync();
                    return Ok(products);
                }

                // Apply filtering when any parameter is provided
                var filteredProducts = await productService.GetProductsAsync(
                    color,
                    size,
                    minPrice,
                    maxPrice,
                    minDiscount,
                    category,
                    stock,
                    sort,
                    pageNumber,
                    pageSize);

                return Ok(filteredProducts);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

    }
}
