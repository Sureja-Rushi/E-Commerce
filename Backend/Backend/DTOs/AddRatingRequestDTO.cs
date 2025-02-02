namespace Backend.DTOs
{
    public class AddRatingRequestDTO
    {
        public int ProductId { get; set; }
        public decimal RatingNumber { get; set; }
    }
}
