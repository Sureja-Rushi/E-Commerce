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
        private readonly ICartRepository cartRepository;

        public UserService(IUserRepository userRepository, IOptions<JwtConfiguration> configuration, ICartRepository cartRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.configuration = configuration.Value;
            this.cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return user;
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
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                ContactNumber = request.ContactNumber,
                CreatedAt = DateTime.UtcNow
            };

            newUser.PasswordHash = passwordHasher.HashPassword(newUser, request.Password);

            var createdUser = await userRepository.CreateUserAsync(newUser);

            await cartRepository.CreateCartAsync(createdUser.Id);

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
                    FirstName = createdUser.FirstName,
                    LastName = createdUser.LastName,
                    Email = createdUser.Email,
                    ContactNumber = createdUser.ContactNumber
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
            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;
            user.Email = updateDto.Email;
            user.Role = updateDto.Role;
            user.ContactNumber = updateDto.ContactNumber;
            user.Role = updateDto.Role;

            var updatedUser = await userRepository.UpdateUserAsync(user);

            return new UserDto
            {
                Id = updatedUser.Id,
                FirstName = updatedUser.FirstName,
                LastName = updatedUser.LastName,
                Email = updatedUser.Email,
                Role = updatedUser.Role,
                ContactNumber = updatedUser.ContactNumber
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
