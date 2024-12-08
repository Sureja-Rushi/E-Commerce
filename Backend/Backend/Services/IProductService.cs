using Backend.DTOs;

namespace Backend.Services
{
    public interface IProductService
    {
        Task<IEnumerable<CreateProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductDetailsAsync(int id);
        Task<IEnumerable<CreateProductDto>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<CreateProductDto>> GetSortedProductsAsync(string sortBy);
        Task AddProductAsync(CreateProductDto product);
        Task UpdateProductAsync(int id, CreateProductDto product);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<CreateProductDto>> SearchProductsAsync(string search);
        Task<IEnumerable<CreateProductDto>> SearchProductsWithFiltersAsync(string search, decimal? minPrice, decimal? maxPrice);
        Task UpdateStockAsync(int productId, int quantityChange);
        Task<bool> CheckStockAvailabilityAsync(int productId, int requiredQuantity);



        //Task<ProductDto> GetProductByIdAsync(int id);
        //Task<IEnumerable<ProductDto>> GetProductsAsync(string search, string category, string sortOrder);
        //Task AddProductAsync(CreateProductDto productDto);
        //Task UpdateProductAsync(int id, CreateProductDto productDto);
        //Task DeleteProductAsync(int id);
    }
}
