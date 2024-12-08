using Backend.Models;

namespace Backend.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
        Task<IEnumerable<Product>> GetSortedProductsAsync(string sortBy);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeletProductAsync(int id);
        Task<IEnumerable<Product>> SearchProductsByNameOrBrandAsync(string search);
        Task<IEnumerable<Product>> SearchProductsWithFiltersAsync(string search, decimal? minPrice, decimal? maxPrice);




        //Task<Product> GetProductByIdAsync(int id);
        //Task<IEnumerable<Product>> GetProductsAsync(string search, string category, string sortOrder);
        //Task AddProductAsync(Product product);
        //Task UpdateProductAsync(Product product);
        //Task DeleteProductAsync(int id);
        //Task<int?> GetCategoryIdByNameAsync(string categoryName);

    }
}
