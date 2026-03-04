using EventBookingSystem.Application.Dtos;

namespace EventBookingSystem.Application.Services.Interfaces
{
    public interface IWalletService
    {
        Task<ApiResponse<string>> InitiateTopUpAsync(InitiateTopUpRequest request, string UserId);
        Task<ApiResponse<bool>> ConfirmTopUpAsync(string reference);
    }
}
