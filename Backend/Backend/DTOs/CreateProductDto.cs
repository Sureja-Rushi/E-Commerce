namespace Backend.DTOs
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Specifications { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
        public string CategoryName { get; set; }
        public int StockQuantity { get; set; }
    }
}
