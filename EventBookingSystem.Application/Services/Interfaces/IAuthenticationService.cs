using EventBookingSystem.Application.Dtos;

namespace EventBookingSystem.Application.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<LoginResponseDto>> CustomerLoginAsync(LoginUserRequest request);
        Task<ApiResponse<LoginResponseDto>> AdminLoginAsync(LoginUserRequest request);
    }
}
