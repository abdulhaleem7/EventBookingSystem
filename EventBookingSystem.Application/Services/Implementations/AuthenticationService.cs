using EventBookingSystem.Application.Dtos;
using EventBookingSystem.Application.Services.Interfaces;
using EventBookingSystem.Infrastructure.Repositories.Interfaces;
using EventBookingSystem.Application.JWT;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using EventBookingSystem.Domain.Constant;

namespace EventBookingSystem.Application.Services.Implementations
{
    public class AuthenticationService(IUserRepository userRepository, IJwtAuthService jwtAuthService,IAdminRepository adminRepository) : IAuthenticationService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJwtAuthService _jwtAuthService = jwtAuthService;
        private readonly IAdminRepository _adminRepository = adminRepository;

        public async Task<ApiResponse<LoginResponseDto>> CustomerLoginAsync(LoginUserRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                return ApiResponse<LoginResponseDto>.NotFound("User not found");

            if (!VerifyPassword(request.Password, user.PasswordHash))
                return ApiResponse<LoginResponseDto>.BadRequest("Invalid credentials");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, Roles.Customer)
            };

            var token = _jwtAuthService.GenerateAccessToken(claims);
            var refresh = _jwtAuthService.GenerateRefreshToken();

            var response = new LoginResponseDto { Token = token, RefreshToken = refresh, ExpiresAt = DateTime.UtcNow.AddHours(1) };
            return ApiResponse<LoginResponseDto>.Ok(response);
        }

        public async Task<ApiResponse<LoginResponseDto>> AdminLoginAsync(LoginUserRequest request)
        {
            var admin = await _adminRepository.GetAsync(x=>x.Email == request.Email);
            if (admin == null)
                return ApiResponse<LoginResponseDto>.NotFound("Admin not found");

            if (!VerifyPassword(request.Password, admin.Password))
                return ApiResponse<LoginResponseDto>.BadRequest("Invalid credentials");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, admin.Id),
                new Claim(ClaimTypes.Email, admin.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, Roles.Admin)
            };

            var token = _jwtAuthService.GenerateAccessToken(claims);
            var refresh = _jwtAuthService.GenerateRefreshToken();

            var response = new LoginResponseDto { Token = token, RefreshToken = refresh, ExpiresAt = DateTime.UtcNow.AddHours(1) };
            return ApiResponse<LoginResponseDto>.Ok(response);
        }

        private bool VerifyPassword(string password, string? storedHash)
        {
            if (string.IsNullOrEmpty(storedHash)) return false;
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            var hashString = Convert.ToBase64String(hash);
            return hashString == storedHash;
        }
    }
}
