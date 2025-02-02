namespace Backend.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveryDate { get; set; }

        public int ShippingAddressId { get; set; }
        public Address ShippingAddress { get; set; }

        // Payment Details
        //public string TransactionId { get; set; }
        //public string PaymentId { get; set; }
        public string PaymentStatus { get; set; } = "Pending";

        // Order Details
        public decimal TotalPrice { get; set; }
        public decimal TotalDiscountedPrice { get; set; }
        public decimal Discount { get; set; }
        public string OrderStatus { get; set; } = "Pending";
        public int TotalItems { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
