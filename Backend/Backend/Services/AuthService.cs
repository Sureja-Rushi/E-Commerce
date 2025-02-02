using Backend.Configurations;
using Backend.DTOs;
using Backend.Helpers;
using Backend.Models;
using Backend.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly JwtConfiguration configuration;

        public AuthService(IUserRepository userRepository, IOptions<JwtConfiguration> configuration)
        {
            this.userRepository = userRepository;
            this.configuration = configuration.Value;
        }

        public async Task<AuthResponse> AuthenticateAsync(AuthRequestDto request)
        {
            var user = await userRepository.GetUserByEmailAsync(request.Email);

            if (user == null) 
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var token = JwtTokenHelper.GenerateToken(user, configuration.SecretKey, configuration.Issuer, configuration.Audience, configuration.ExpirationMinutes);
            var response = new AuthResponse
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ContactNumber = user.ContactNumber,
                    Email = user.Email,
                    Role = user.Role
                }
            };

            return response;
        }
    }
}
