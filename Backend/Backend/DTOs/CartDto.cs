namespace Backend.DTOs
{
    public class CartDto
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string GuestCartId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CartItemDto> CartItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
