using EventBookingSystem.Application.Dtos;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<ApiResponse<UserResponseDto>> CreateUserAsync(CreateUserRequest request);
        Task<ApiResponse<UserResponseDto>> GetCustomerDetailsAsync(string userId);
    }
}
