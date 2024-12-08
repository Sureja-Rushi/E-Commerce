using Backend.DTOs;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository productRepository;
        private readonly IReviewRepository reviewRepository;
        private readonly ICategoryRepository categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IReviewRepository reviewRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.reviewRepository = reviewRepository;
        }

        public async Task<IEnumerable<CreateProductDto>> GetAllProductsAsync()
        {
            var products = await productRepository.GetAllProductsAsync();
            return products.Select(MapToCreateProductDto);
        }

        public async Task<ProductDto> GetProductDetailsAsync(int id)
        {
            var product = await productRepository.GetProductByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found.");

            return MapToProductDto(product);
        }

        public async Task<IEnumerable<CreateProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await productRepository.GetProductsByCategoryAsync(categoryId);
            return products.Select(MapToCreateProductDto);
        }

        public async Task<IEnumerable<CreateProductDto>> GetSortedProductsAsync(string sortBy)
        {
            var products = await productRepository.GetSortedProductsAsync(sortBy);
            return products.Select(MapToCreateProductDto);
        }

        public async Task AddProductAsync(CreateProductDto productDto)
        {
            var category = await categoryRepository.GetCategoryByNameAsync(productDto.CategoryName);
            if (category == null)
                throw new Exception("Category not found.");

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Specifications = productDto.Specifications,
                Price = productDto.Price,
                Brand = productDto.Brand,
                CategoryId = category.Id,
                Category = category,
                StockQuantity = productDto.StockQuantity
            };

            await productRepository.AddProductAsync(product);
        }

        public async Task UpdateProductAsync(int id, CreateProductDto productDto)
        {
            var product = await productRepository.GetProductByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found.");

            var category = await categoryRepository.GetCategoryByNameAsync(productDto.CategoryName);
            if (category == null)
                throw new Exception("Category not found.");

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Specifications = productDto.Specifications;
            product.Price = productDto.Price;
            product.Brand = productDto.Brand;
            product.CategoryId = category.Id;
            product.StockQuantity = productDto.StockQuantity;

            await productRepository.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await productRepository.GetProductByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found.");

            await productRepository.DeletProductAsync(id);
        }

        private CreateProductDto MapToCreateProductDto(Product product)
        {
            return new CreateProductDto
            {
                Name = product.Name,
                Description = product.Description,
                Specifications = product.Specifications,
                Price = product.Price,
                Brand = product.Brand,
                CategoryName = product.Category.Name,
                StockQuantity = product.StockQuantity,
            };
        }

        private ProductDto MapToProductDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Specifications = product.Specifications,
                Price = product.Price,
                Brand = product.Brand,
                CategoryName = product.Category.Name,
                StockQuantity = product.StockQuantity,
                // Safely handle null reviews
                Reviews = product.Reviews?.Select(r => new ReviewDto
                {
                    Comment = r.Comment,
                    Rating = r.Rating,
                    UserName = r.User?.FullName // Ensure that User is not null
                }).ToList() ?? new List<ReviewDto>() // If Reviews is null, return an empty list
            };
        }

        public async Task<IEnumerable<CreateProductDto>> SearchProductsAsync(string search)
        {
            var products = await productRepository.SearchProductsByNameOrBrandAsync(search);
            return products.Select(MapToCreateProductDto);
        }

        public async Task<IEnumerable<CreateProductDto>> SearchProductsWithFiltersAsync(string search, decimal? minPrice, decimal? maxPrice)
        {
            var products = await productRepository.SearchProductsWithFiltersAsync(search, minPrice, maxPrice);
            return products.Select(MapToCreateProductDto);
        }

        public async Task UpdateStockAsync(int productId, int quantityChange)
        {
            var product = await productRepository.GetProductByIdAsync(productId);
            if (product == null)
                throw new Exception("Product not found.");

            if (product.StockQuantity + quantityChange < 0)
                throw new Exception("Insufficient stock.");

            product.StockQuantity += quantityChange;
            await productRepository.UpdateProductAsync(product);
        }

        public async Task<bool> CheckStockAvailabilityAsync(int productId, int requiredQuantity)
        {
            var product = await productRepository.GetProductByIdAsync(productId);
            if (product == null)
                throw new Exception("Product not found.");

            return product.StockQuantity >= requiredQuantity;
        }


    }
}
