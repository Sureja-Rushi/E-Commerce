using Backend.Models;

namespace Backend.DTOs
{
    public class UpdateProductRequestDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DiscountedPrice { get; set; }

        public decimal DiscountPercent { get; set; }

        public int Quantity { get; set; }
        public string Brand { get; set; }

        public string Color { get; set; }

        public string ImageUrl { get; set; }

        public string CategoryName { get; set; }
        public string ParentCategoryName { get; set; }

        public string GrandParentCategoryName { get; set; }

        public List<SizeDTO> Sizes { get; set; }
    }
}
