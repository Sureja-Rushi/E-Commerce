using Backend.DTOs;

namespace Backend.Models
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}
