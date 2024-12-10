namespace Backend.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; } // Link to the Order
        public int ProductId { get; set; } // Product being purchased
        public int Quantity { get; set; } // Number of units purchased
        public decimal Price { get; set; } // Price of the product at the time of order
        public decimal TotalPrice { get; set; } // Total price for the quantity of this product

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
