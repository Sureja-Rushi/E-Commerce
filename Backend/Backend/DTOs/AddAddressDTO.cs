namespace Backend.DTOs
{
    public class AddAddressDTO
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string FirstName { get; set; } // Added FirstName
        public string LastName { get; set; } // Added LastName
        public string ContactNumber { get; set; }
    }
}
