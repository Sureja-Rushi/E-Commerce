namespace Backend.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
