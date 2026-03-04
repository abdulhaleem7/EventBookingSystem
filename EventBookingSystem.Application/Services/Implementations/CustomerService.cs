using EventBookingSystem.Application.Dtos;
using EventBookingSystem.Application.Services.Interfaces;
using EventBookingSystem.Infrastructure.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace EventBookingSystem.Application.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;

        public CustomerService(IUserRepository userRepository, IWalletRepository walletRepository)
        {
            _userRepository = userRepository;
            _walletRepository = walletRepository;
        }

        public async Task<ApiResponse<UserResponseDto>> CreateUserAsync(CreateUserRequest request)
        {
            // basic validation
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return ApiResponse<UserResponseDto>.BadRequest("Email and password are required.");

            var exists = await _userRepository.Exist(u => u.Email == request.Email);
            if (exists)
                return ApiResponse<UserResponseDto>.BadRequest("A user with that email already exists.");

            // create user
            var user = new EventBookingSystem.Domain.Models.User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                FullName = string.IsNullOrWhiteSpace(request.FirstName) ? request.LastName : (request.FirstName + " " + request.LastName),
                PasswordHash = HashPassword(request.Password)
            };

            await _userRepository.AddAsync(user);

            // create wallet for user
            var wallet = new EventBookingSystem.Domain.Models.Wallet
            {
                UserId = user.Id,
                Balance = 0m
            };

            await _walletRepository.AddAsync(wallet);

            // persist
            await _userRepository.SaveChangesAsync();

            var dto = new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                WalletBalance = wallet.Balance
            };

            return ApiResponse<UserResponseDto>.Ok(dto);
        }

        public async Task<ApiResponse<UserResponseDto>> GetCustomerDetailsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return ApiResponse<UserResponseDto>.BadRequest("Invalid user id.");

            var user = await _userRepository.GetAsync(u => u.Id == userId);
            if (user == null)
                return ApiResponse<UserResponseDto>.NotFound("User not found.");

            var wallet = await _walletRepository.GetByUserIdAsync(userId);

            var dto = new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                WalletBalance = wallet?.Balance ?? 0m
            };

            return ApiResponse<UserResponseDto>.Ok(dto);
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
