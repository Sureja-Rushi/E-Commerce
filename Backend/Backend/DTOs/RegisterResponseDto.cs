namespace Backend.DTOs
{
    public class RegisterResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
