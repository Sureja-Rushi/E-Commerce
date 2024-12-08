using Backend.DTOs;

namespace Backend.Services
{
    public interface IUserService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);

        Task<UserDto> GetUserByIdAsync(int userId);

        Task<UserDto> UpdateUserAsync(int userId, UserDto updateDto);

        Task DeleteUserAsync(int userId);
    }
}
