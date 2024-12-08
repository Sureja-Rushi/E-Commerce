using Azure;
using Backend.Configurations;
using Backend.DTOs;
using Backend.Helpers;
using Backend.Models;
using Backend.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly JwtConfiguration configuration;

        public UserService(IUserRepository userRepository, IOptions<JwtConfiguration> configuration)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.configuration = configuration.Value;
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (await userRepository.UserExistsAsync(request.Email))
            {
                throw new Exception("User with this Email already exists.");
            }

            var passwordHasher = new PasswordHasher<User>();

            var newUser = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Role = string.IsNullOrEmpty(request.Role) ? "User" : request.Role,
                CreatedAt = DateTime.UtcNow
            };

            newUser.PasswordHash = passwordHasher.HashPassword(newUser, request.Password);

            var createdUser = await userRepository.CreateUserAsync(newUser);

            // Generate JWT token
            var token = JwtTokenHelper.GenerateToken(
                createdUser,
                configuration.SecretKey,
                configuration.Issuer,
                configuration.Audience,
                configuration.ExpirationMinutes);

            return new RegisterResponseDto
            {
                User = new UserDto
                {
                    Id = createdUser.Id,
                    FullName = createdUser.FullName,
                    Email = createdUser.Email,
                    Role = createdUser.Role
                },
                Token = token
            };
        }

        public async Task<UserDto> UpdateUserAsync(int userId, UserDto updateDto)
        {
            var user = await userRepository.GetUserByIdAsync(userId);

            if(user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            user.FullName = updateDto.FullName;
            user.Email = updateDto.Email;
            user.Role = updateDto.Role;

            var updatedUser = await userRepository.UpdateUserAsync(user);

            return new UserDto
            {
                Id = updatedUser.Id,
                FullName = updatedUser.FullName,
                Email = updatedUser.Email,
                Role = updatedUser.Role
            };
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            await userRepository.DeleteUserAsync(user);
        }

    }
}
