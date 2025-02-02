namespace Backend.Models
{
    public class Category
    {
        //public int Id { get; set; }
        //public string Name { get; set; }

        //public ICollection<Product> Products { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; } // Nullable for top-level categories
        public Category ParentCategory { get; set; } // Self-referencing relationship
        public int Level { get; set; }

        public ICollection<Category> SubCategories { get; set; }
    }
}
