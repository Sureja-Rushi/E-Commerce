using Backend.DTOs;
using Backend.Helpers;
using Backend.Models;

namespace Backend.Services
{
    public interface IProductService
    {
        //Task<IEnumerable<CreateProductDto>> GetAllProductsAsync();
        //Task<ProductDto> GetProductDetailsAsync(int id);
        //Task<IEnumerable<CreateProductDto>> GetProductsByCategoryAsync(int categoryId);
        //Task<IEnumerable<CreateProductDto>> GetSortedProductsAsync(string sortBy);
        //Task AddProductAsync(CreateProductDto product);
        //Task UpdateProductAsync(int id, CreateProductDto product);
        //Task DeleteProductAsync(int id);
        //Task<IEnumerable<CreateProductDto>> SearchProductsAsync(string search);
        //Task<IEnumerable<CreateProductDto>> SearchProductsWithFiltersAsync(string search, decimal? minPrice, decimal? maxPrice);
        //Task UpdateStockAsync(int productId, int quantityChange);
        //Task<bool> CheckStockAvailabilityAsync(int productId, int requiredQuantity);



        //Task<ProductDto> GetProductByIdAsync(int id);
        //Task<IEnumerable<ProductDto>> GetProductsAsync(string search, string category, string sortOrder);
        //Task AddProductAsync(CreateProductDto productDto);
        //Task UpdateProductAsync(int id, CreateProductDto productDto);
        //Task DeleteProductAsync(int id);

        Task<bool> AddProductAsync(Product product, string categoryName, string parentCategoryName, string grandParentCategoryName, List<ProductSize> productSizes);
        Task<bool> DeleteProductAsync(int productId);
        Task<bool> UpdateProductAsync(
            int productId,
            string title,
            string description,
            decimal price,
            decimal discountedPrice,
            decimal discountPercent,
            int quantity,
            string brand,
            string color,
            string imageUrl,
            string categoryName,
            string parentCategoryName,
            string grandParentCategoryName,
            List<SizeDTO> sizes);

        Task<Product> GetProductByIdAsync(int productId);

        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<PagedResult<Product>> GetProductsAsync(string? color, string? size, decimal? minPrice, decimal? maxPrice, decimal? minDiscount, string? category, bool? stock, string? sort, int pageNumber, int PageSize);
    }
}
