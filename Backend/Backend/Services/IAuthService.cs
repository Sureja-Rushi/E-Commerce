using Backend.DTOs;
using Backend.Models;

namespace Backend.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync(AuthRequestDto request);

    }
}
