namespace Backend.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal TotalPrice { get; set; } = 0;
        public decimal TotalDiscountedPrice { get; set; } = 0;
        public int TotalItems { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
    }
}
