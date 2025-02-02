using Backend.DTOs;
using Backend.Models;
using Backend.Repositories;
using Backend.Helpers;

namespace Backend.Services
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository productRepository;
        //private readonly IReviewRepository reviewRepository;
        private readonly ICategoryRepository categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            //this.reviewRepository = reviewRepository;
        }

        //public async Task<IEnumerable<CreateProductDto>> GetAllProductsAsync()
        //{
        //    var products = await productRepository.GetAllProductsAsync();
        //    return products.Select(MapToCreateProductDto);
        //}

        //public async Task<ProductDto> GetProductDetailsAsync(int id)
        //{
        //    var product = await productRepository.GetProductByIdAsync(id);
        //    if (product == null)
        //        throw new Exception("Product not found.");

        //    return MapToProductDto(product);
        //}

        //public async Task<IEnumerable<CreateProductDto>> GetProductsByCategoryAsync(int categoryId)
        //{
        //    var products = await productRepository.GetProductsByCategoryAsync(categoryId);
        //    return products.Select(MapToCreateProductDto);
        //}

        //public async Task<IEnumerable<CreateProductDto>> GetSortedProductsAsync(string sortBy)
        //{
        //    var products = await productRepository.GetSortedProductsAsync(sortBy);
        //    return products.Select(MapToCreateProductDto);
        //}

        //public async Task AddProductAsync(CreateProductDto productDto)
        //{
        //    var category = await categoryRepository.GetCategoryByNameAsync(productDto.CategoryName);
        //    if (category == null)
        //        throw new Exception("Category not found.");

        //    var product = new Product
        //    {
        //        Name = productDto.Name,
        //        Description = productDto.Description,
        //        Specifications = productDto.Specifications,
        //        Price = productDto.Price,
        //        Brand = productDto.Brand,
        //        CategoryId = category.Id,
        //        Category = category,
        //        StockQuantity = productDto.StockQuantity
        //    };

        //    await productRepository.AddProductAsync(product);
        //}

        //public async Task UpdateProductAsync(int id, CreateProductDto productDto)
        //{
        //    var product = await productRepository.GetProductByIdAsync(id);
        //    if (product == null)
        //        throw new Exception("Product not found.");

        //    var category = await categoryRepository.GetCategoryByNameAsync(productDto.CategoryName);
        //    if (category == null)
        //        throw new Exception("Category not found.");

        //    product.Name = productDto.Name;
        //    product.Description = productDto.Description;
        //    product.Specifications = productDto.Specifications;
        //    product.Price = productDto.Price;
        //    product.Brand = productDto.Brand;
        //    product.CategoryId = category.Id;
        //    product.StockQuantity = productDto.StockQuantity;

        //    await productRepository.UpdateProductAsync(product);
        //}

        //public async Task DeleteProductAsync(int id)
        //{
        //    var product = await productRepository.GetProductByIdAsync(id);
        //    if (product == null)
        //        throw new Exception("Product not found.");

        //    await productRepository.DeletProductAsync(id);
        //}

        //private CreateProductDto MapToCreateProductDto(Product product)
        //{
        //    return new CreateProductDto
        //    {
        //        Name = product.Name,
        //        Description = product.Description,
        //        Specifications = product.Specifications,
        //        Price = product.Price,
        //        Brand = product.Brand,
        //        CategoryName = product.Category.Name,
        //        StockQuantity = product.StockQuantity,
        //    };
        //}

        //private ProductDto MapToProductDto(Product product)
        //{
        //    return new ProductDto
        //    {
        //        Id = product.Id,
        //        Name = product.Name,
        //        Description = product.Description,
        //        Specifications = product.Specifications,
        //        Price = product.Price,
        //        Brand = product.Brand,
        //        CategoryName = product.Category.Name,
        //        StockQuantity = product.StockQuantity,
        //        // Safely handle null reviews
        //        Reviews = product.Reviews?.Select(r => new ReviewDto
        //        {
        //            Comment = r.Comment,
        //            Rating = r.Rating,
        //            UserName = r.User.FirstName // Ensure that User is not null
        //        }).ToList() ?? new List<ReviewDto>() // If Reviews is null, return an empty list
        //    };
        //}

        //public async Task<IEnumerable<CreateProductDto>> SearchProductsAsync(string search)
        //{
        //    var products = await productRepository.SearchProductsByNameOrBrandAsync(search);
        //    return products.Select(MapToCreateProductDto);
        //}

        //public async Task<IEnumerable<CreateProductDto>> SearchProductsWithFiltersAsync(string search, decimal? minPrice, decimal? maxPrice)
        //{
        //    var products = await productRepository.SearchProductsWithFiltersAsync(search, minPrice, maxPrice);
        //    return products.Select(MapToCreateProductDto);
        //}

        //public async Task UpdateStockAsync(int productId, int quantityChange)
        //{
        //    var product = await productRepository.GetProductByIdAsync(productId);
        //    if (product == null)
        //        throw new Exception("Product not found.");

        //    if (product.StockQuantity + quantityChange < 0)
        //        throw new Exception("Insufficient stock.");

        //    product.StockQuantity += quantityChange;
        //    await productRepository.UpdateProductAsync(product);
        //}

        //public async Task<bool> CheckStockAvailabilityAsync(int productId, int requiredQuantity)
        //{
        //    var product = await productRepository.GetProductByIdAsync(productId);
        //    if (product == null)
        //        throw new Exception("Product not found.");

        //    return product.StockQuantity >= requiredQuantity;
        //}

        public async Task<bool> AddProductAsync(Product product, string categoryName, string parentCategoryName, string grandParentCategoryName, List<ProductSize> productSizes)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (string.IsNullOrEmpty(categoryName) ||
                string.IsNullOrEmpty(parentCategoryName) ||
                string.IsNullOrEmpty(grandParentCategoryName))
            {
                throw new ArgumentException("Category names must not be null or empty.");
            }

            return await productRepository.AddProductAsync(
                product,
                categoryName,
                parentCategoryName,
                grandParentCategoryName,
                productSizes);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            return await productRepository.DeleteProductAsync(productId);
        }

        public async Task<bool> UpdateProductAsync(
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
            List<SizeDTO> sizes)
        {
            // Validate category hierarchy and get third-level category
            var thirdLevelCategory = await productRepository.ValidateAndCreateCategoryHierarchy(
                categoryName,
                parentCategoryName,
                grandParentCategoryName);

            if (thirdLevelCategory == null)
            {
                throw new Exception("Category validation failed.");
            }

            // Validate size quantities
            if (sizes != null && sizes.Any())
            {
                var totalSizeQuantity = sizes.Sum(s => s.Quantity);
                if (totalSizeQuantity > quantity)
                {
                    throw new ArgumentException("Product quantity must be greater than or equal to the sum of all size quantities.");
                }
            }

            return await productRepository.UpdateProductAsync(
                productId,
                title,
                description,
                price,
                discountedPrice,
                discountPercent,
                quantity,
                brand,
                color,
                imageUrl,
                thirdLevelCategory.Id,
                sizes);
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            if (productId <= 0)
            {
                throw new ArgumentException("Product ID must be greater than 0.");
            }

            var product = await productRepository.GetProductByIdAsync(productId);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not Found.");
            }

            return product;

        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = await productRepository.GetAllProductsAsync();

            if (!products.Any())
            {
                throw new KeyNotFoundException("No products found.");
            }

            return products;
        }

        public async Task<PagedResult<Product>> GetProductsAsync(string? color, string? size, decimal? minPrice, decimal? maxPrice, decimal? minDiscount, string? category, bool? stock, string? sort, int pageNumber, int PageSize)
        {
            return await productRepository.GetProductsAsync(
                color,
                size,
                minPrice,
                maxPrice,
                minDiscount,
                category,
                stock,
                sort,
                pageNumber,
                PageSize);
        }

    }
}
