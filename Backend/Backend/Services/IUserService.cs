using Backend.DTOs;
using Backend.Models;

namespace Backend.Services
{
    public interface IUserService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);

        Task<User> GetUserByIdAsync(int userId);

        Task<UserDto> UpdateUserAsync(int userId, UserDto updateDto);

        Task DeleteUserAsync(int userId);
    }
}
