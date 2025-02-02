using System.ComponentModel;

namespace Backend.Models
{
    public class Product
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public string Specifications { get; set; }
        //public decimal Price { get; set; }
        //public string Brand { get; set; }
        //public int CategoryId { get; set; }
        //public Category Category { get; set; }
        //public int StockQuantity { get; set; }
        //public ICollection<Review> Reviews { get; set; }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public int Quantity { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public ICollection<ProductSize> Sizes { get; set; } // Nullable for products with no sizes
        public string ImageUrl { get; set; }
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>(); // One-to-many relationship
        public ICollection<Review> Reviews { get; set; } = new List<Review>(); // One-to-many relationship
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
