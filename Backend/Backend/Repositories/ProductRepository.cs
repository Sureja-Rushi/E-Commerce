using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Backend.Helpers;

namespace Backend.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddProductAsync(
    Product product,
    string categoryName,
    string parentCategoryName,
    string grandParentCategoryName,
    List<ProductSize> productSizes = null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Validate total size quantity against product quantity
                if (productSizes != null && productSizes.Any())
                {
                    var totalSizeQuantity = productSizes.Sum(size => size.Quantity);
                    if (totalSizeQuantity > product.Quantity)
                    {
                        throw new ArgumentException("Product quantity must be greater than or equal to the sum of all size quantities.");
                    }
                }

                // Check for the third-level category
                var thirdLevelCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == categoryName);

                if (thirdLevelCategory == null)
                {
                    // Check and create second-level parent category
                    var secondLevelCategory = await _context.Categories
                        .FirstOrDefaultAsync(c => c.Name == parentCategoryName);

                    if (secondLevelCategory == null)
                    {
                        // Check and create first-level parent category
                        var firstLevelCategory = await _context.Categories
                            .FirstOrDefaultAsync(c => c.Name == grandParentCategoryName);

                        if (firstLevelCategory == null)
                        {
                            firstLevelCategory = new Category
                            {
                                Name = grandParentCategoryName,
                                Level = 1
                            };
                            _context.Categories.Add(firstLevelCategory);
                            await _context.SaveChangesAsync();
                        }

                        secondLevelCategory = new Category
                        {
                            Name = parentCategoryName,
                            Level = 2,
                            ParentCategoryId = firstLevelCategory.Id
                        };
                        _context.Categories.Add(secondLevelCategory);
                        await _context.SaveChangesAsync();
                    }

                    thirdLevelCategory = new Category
                    {
                        Name = categoryName,
                        Level = 3,
                        ParentCategoryId = secondLevelCategory.Id
                    };
                    _context.Categories.Add(thirdLevelCategory);
                    await _context.SaveChangesAsync();
                }

                // Add the product with the third-level category
                product.CategoryId = thirdLevelCategory.Id;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                // Add sizes if provided
                if (productSizes != null && productSizes.Any())
                {
                    foreach (var size in productSizes)
                    {
                        size.ProductId = product.Id;
                        _context.ProductSizes.Add(size);
                    }
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                return false; // Product not found
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
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
            int categoryId,
            List<SizeDTO> sizes)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var product = await _context.Products.Include(p => p.Sizes).FirstOrDefaultAsync(p => p.Id == productId);

                if (product == null)
                {
                    return false; // Product not found
                }

                // Update product properties
                product.Title = title;
                product.Description = description;
                product.Price = price;
                product.DiscountedPrice = discountedPrice;
                product.DiscountPercent = discountPercent;
                product.Quantity = quantity;
                product.Brand = brand;
                product.Color = color;
                product.ImageUrl = imageUrl;
                product.CategoryId = categoryId;

                // Handle size updates
                if (sizes != null && sizes.Any())
                {
                    // Remove existing sizes
                    _context.ProductSizes.RemoveRange(product.Sizes);

                    // Add updated sizes
                    foreach (var size in sizes)
                    {
                        _context.ProductSizes.Add(new ProductSize
                        {
                            ProductId = product.Id,
                            SizeName = size.SizeName,
                            Quantity = size.Quantity
                        });
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Sizes)
                .Include(p => p.Ratings)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Sizes)
                .ToListAsync();
        }

        //color=blue&size=8&minPrice=10&maxPrice=100&minDiscount=0&category=Sports Shoes&stock=true&sort=price_low&pageNumber=1&pageSize=10
        public async Task<PagedResult<Product>> GetProductsAsync(string? color, string? size, decimal? minPrice, decimal? maxPrice, decimal? minDiscount, string? category, bool? stock, string? sort, int pageNumber, int PageSize)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Sizes)
                .AsQueryable();

            if(!string.IsNullOrEmpty(color))
            {
                var colors = color.Split(',').Select(c => c.Trim().ToLower()).ToList();
                query = query.Where(p => colors.Contains(p.Color.ToLower()));
            }

            if(!string.IsNullOrEmpty(size))
            {
                query = query.Where(p => p.Sizes.Any(s => s.SizeName.ToLower() == size.ToLower()));
            }

            if(minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (minDiscount.HasValue)
            {
                query = query.Where(p => p.DiscountPercent >= minDiscount.Value);
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.Name.ToLower() == category.ToLower());
            }

            if (stock.HasValue)
            {
                query = query.Where(p => stock.Value ? p.Quantity > 0 : p.Quantity <= 0);
            }

            query = sort switch
            {
                "price_low" => query.OrderBy(p => p.Price),
                "price_high" => query.OrderByDescending(p => p.Price),
                _ => query
            };

            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return new PagedResult<Product>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = PageSize
            };
        }

        public async Task<Category> ValidateAndCreateCategoryHierarchy(
            string categoryName,
            string parentCategoryName,
            string grandParentCategoryName)
        {
            // Check for the third-level category
            var thirdLevelCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == categoryName);

            if (thirdLevelCategory == null)
            {
                // Check and create second-level parent category
                var secondLevelCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == parentCategoryName);

                if (secondLevelCategory == null)
                {
                    // Check and create first-level parent category
                    var firstLevelCategory = await _context.Categories
                        .FirstOrDefaultAsync(c => c.Name == grandParentCategoryName);

                    if (firstLevelCategory == null)
                    {
                        firstLevelCategory = new Category
                        {
                            Name = grandParentCategoryName,
                            Level = 1
                        };
                        _context.Categories.Add(firstLevelCategory);
                        await _context.SaveChangesAsync();
                    }

                    secondLevelCategory = new Category
                    {
                        Name = parentCategoryName,
                        Level = 2,
                        ParentCategoryId = firstLevelCategory.Id
                    };
                    _context.Categories.Add(secondLevelCategory);
                    await _context.SaveChangesAsync();
                }

                thirdLevelCategory = new Category
                {
                    Name = categoryName,
                    Level = 3,
                    ParentCategoryId = secondLevelCategory.Id
                };
                _context.Categories.Add(thirdLevelCategory);
                await _context.SaveChangesAsync();
            }

            return thirdLevelCategory;
        }



        //public async Task<IEnumerable<Product>> GetAllProductsAsync()
        //{
        //    return await _context.Products
        //        .Include(p => p.Category)
        //        .Include(p => p.Reviews)
        //        .ToListAsync();
        //}

        //public async Task<Product> GetProductByIdAsync(int id)
        //{
        //    return await _context.Products
        //        .Include(p => p.Category)
        //        .Include(p => p.Reviews)
        //        .FirstOrDefaultAsync(p => p.Id == id);
        //}

        //public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        //{
        //    return await _context.Products
        //        .Include(p => p.Category)
        //        .Where(p => p.CategoryId == categoryId)
        //        .ToListAsync();
        //}

        //public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        //{
        //    return await _context.Products
        //        .Include(p => p.Category)
        //        .Where(p => p.Name.Contains(name))
        //        .ToListAsync();
        //}

        //public async Task<IEnumerable<Product>> GetSortedProductsAsync(string sortBy)
        //{
        //    IQueryable<Product> query = _context.Products.Include(p => p.Category);

        //    query = sortBy switch
        //    {
        //        "name" => query.OrderBy(p => p.Name),
        //        "price_asc" => query.OrderBy(p => p.Price),
        //        "price_desc" => query.OrderByDescending(p => p.Price),
        //        "category" => query.OrderBy(p => p.Category.Name),
        //        _ => query // Default order (no sorting)
        //    };

        //    return await query.ToListAsync();
        //}

        //public async Task AddProductAsync(Product product)
        //{
        //    await _context.Products.AddAsync(product);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task UpdateProductAsync(Product product)
        //{
        //    _context.Products.Update(product);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task DeletProductAsync(int id)
        //{
        //    var product = await _context.Products.FindAsync(id);
        //    if (product != null)
        //    {
        //        _context.Products.Remove(product);
        //        await _context.SaveChangesAsync();
        //    }
        //}

        //public async Task<IEnumerable<Product>> SearchProductsByNameOrBrandAsync(string search)
        //{
        //    return await _context.Products
        //        .Include(p => p.Category)
        //        .Where(p => p.Name.Equals(search) || p.Brand.Equals(search) ||
        //                    p.Name.Contains(search) || p.Brand.Contains(search))
        //        .ToListAsync();
        //}

        //public async Task<IEnumerable<Product>> SearchProductsWithFiltersAsync(string search, decimal? minPrice, decimal? maxPrice)
        //{
        //    var query = _context.Products
        //        .Include(p => p.Category)
        //        .AsQueryable();

        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        query = query.Where(p => p.Name.Equals(search) ||
        //                                 p.Brand.Equals(search) ||
        //                                 p.Name.Contains(search) ||
        //                                 p.Brand.Contains(search));
        //    }

        //    if (minPrice.HasValue)
        //    {
        //        query = query.Where(p => p.Price >= minPrice.Value);
        //    }

        //    if (maxPrice.HasValue)
        //    {
        //        query = query.Where(p => p.Price <= maxPrice.Value);
        //    }

        //    return await query.ToListAsync();
        //}



    }
}
