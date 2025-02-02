namespace Backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "Customer";
        public string ContactNumber { get; set; } = "1234567890";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
