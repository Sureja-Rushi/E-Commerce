namespace Backend.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Link to the User who placed the order
        public string ShippingAddress { get; set; } // Address to ship the order
        public decimal TotalAmount { get; set; } // Total order price
        public string OrderStatus { get; set; } // Pending, In Progress, etc.
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
