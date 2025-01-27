using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Exceptions;

namespace Backend.Services
{
    public interface IAuthService
    {
        Task<User> Register(RegisterUserDto userDto);
        Task<AuthResponse> Login(LoginDto loginDto);
        Task InitiatePasswordReset(ForgotPasswordDto dto);
        Task ResetPassword(ResetPasswordDto dto);
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<User> Register(RegisterUserDto userDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                throw new BadRequestException("Email already exists");

            var user = new User
            {
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<AuthResponse> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                throw new UnauthorizedException("Invalid credentials");

            var token = GenerateJwtToken(user);
            return new AuthResponse(token, user.Email);
        }

        public async Task InitiatePasswordReset(ForgotPasswordDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user != null)
            {
                var token = new PasswordResetToken
                {
                    Token = Guid.NewGuid().ToString(),
                    Email = dto.Email,
                    ExpiresAt = DateTime.UtcNow.AddHours(1),
                    IsUsed = false
                };

                _context.PasswordResetTokens.Add(token);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Password reset token for {dto.Email}: {token.Token}");
            }
        }

        public async Task ResetPassword(ResetPasswordDto dto)
        {
            var resetToken = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.Token == dto.Token &&
                                       t.ExpiresAt > DateTime.UtcNow &&
                                       !t.IsUsed);

            if (resetToken == null)
                throw new BadRequestException("Invalid or expired token");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == resetToken.Email)
                ?? throw new NotFoundException("User not found");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            resetToken.IsUsed = true;

            await _context.SaveChangesAsync();
        }

        private string GenerateJwtToken(User user)
        {
            var jwtKey = _config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("userId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}