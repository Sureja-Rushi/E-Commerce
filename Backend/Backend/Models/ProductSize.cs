namespace Backend.Models
{
    public class ProductSize
    {
        public int Id { get; set; }
        public int? ProductId { get; set; } // Nullable for products without sizes
        public Product Product { get; set; }
        public string SizeName { get; set; }
        public int Quantity { get; set; }
    }
}
