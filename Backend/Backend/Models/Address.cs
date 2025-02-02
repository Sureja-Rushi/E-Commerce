namespace Backend.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public string FirstName { get; set; } // Added FirstName
        public string LastName { get; set; } // Added LastName
        public string ContactNumber { get; set; }

        // Foreign key
        public int UserId { get; set; }

        // Navigation property
        public User? User { get; set; }
    }
}
