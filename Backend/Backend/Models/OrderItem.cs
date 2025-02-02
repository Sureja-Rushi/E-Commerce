namespace Backend.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        // Order Relationship
        public int OrderId { get; set; }
        public Order Order { get; set; }

        // Product Relationship
        public int ProductId { get; set; }
        public Product Product { get; set; }

        // User Relationship
        public int UserId { get; set; }
        public User User { get; set; }

        // Item Details
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string Size { get; set; }
    }
}
