namespace Backend.DTOs
{
    public class OrderDetailsDto
    {
        public int OrderId { get; set; }
        public string ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
