using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quizes.Api.Data;
using Quizes.Api.Data.Entities;
using Quizes.Shared;
using Quizes.Shared.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Quizes.Api.Services
{
    public class AuthService
    {
        private readonly QuizContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(QuizContext context, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _context
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == dto.Username);

            if (user == null)
            {
                return new AuthResponseDto(default!, "Невалиден потребител.");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return new AuthResponseDto(default!, "Грешна парола.");
            }

            var token = GenerateJwtToken(user);
            var loggedInUser = new LoggedInUser(user.Id, user.Name, user.Role, token);
            return new AuthResponseDto(loggedInUser);
        }

        private string GenerateJwtToken(User user)
        {
            Claim[] claims =
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Role, user.Role),
            };

            var secret = _configuration.GetValue<string>("Jwt:Secret");

            var symetrycKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret!));

            var signingCred = new SigningCredentials(symetrycKey, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                audience: _configuration.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:ExpiresInDays")),
                signingCredentials: signingCred
                );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return token;
        }
    }
}
