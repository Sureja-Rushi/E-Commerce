﻿using System.ComponentModel;

namespace Backend.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Specifications { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int StockQuantity { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
