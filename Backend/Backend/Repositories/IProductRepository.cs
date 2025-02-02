using Backend.Models;
using Backend.DTOs;
using Backend.Helpers;

namespace Backend.Repositories
{
    public interface IProductRepository
    {
        //Task<IEnumerable<Product>> GetAllProductsAsync();
        //Task<Product> GetProductByIdAsync(int id);
        //Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        //Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
        //Task<IEnumerable<Product>> GetSortedProductsAsync(string sortBy);
        //Task AddProductAsync(Product product);
        //Task UpdateProductAsync(Product product);
        //Task DeletProductAsync(int id);
        //Task<IEnumerable<Product>> SearchProductsByNameOrBrandAsync(string search);
        //Task<IEnumerable<Product>> SearchProductsWithFiltersAsync(string search, decimal? minPrice, decimal? maxPrice);




        //Task<Product> GetProductByIdAsync(int id);
        //Task<IEnumerable<Product>> GetProductsAsync(string search, string category, string sortOrder);
        //Task AddProductAsync(Product product);
        //Task UpdateProductAsync(Product product);
        //Task DeleteProductAsync(int id);
        //Task<int?> GetCategoryIdByNameAsync(string categoryName);

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
            int categoryId,
            List<SizeDTO> sizes);

        Task<Category> ValidateAndCreateCategoryHierarchy(string categoryName, string parentCategoryName, string grandParentCategoryName);
        Task<Product> GetProductByIdAsync(int productId);
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<PagedResult<Product>> GetProductsAsync(string? color, string? size, decimal? minPrice, decimal? maxPrice, decimal? minDiscount, string? category, bool? stock, string? sort, int pageNumber, int PageSize);
    }
}
