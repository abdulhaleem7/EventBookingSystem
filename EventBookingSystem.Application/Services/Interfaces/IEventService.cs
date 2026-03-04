using EventBookingSystem.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Services.Interfaces
{
    public interface IEventService
    {
        Task<ApiResponse<EventResponseDto>> CreateEventAsync(CreateEventRequest request);
        Task<ApiResponse<IList<EventResponseDto>>> GetAllAsync();
        Task<ApiResponse<EventResponseDto>> GetByIdAsync(string id);
        Task<ApiResponse<EventResponseDto>> UpdateEventAsync(UpdateEventRequest request);
        Task<ApiResponse<bool>> DeleteEventAsync(string id);
        Task<ApiResponse<IList<EventResponseDto>>> GetAvailableEventsAsync();
        Task<ApiResponse<string>> BookTicketAsync(CreateBookingRequest request, string userId);
        Task<ApiResponse<string>> CancelBookingAsync(string bookingId,string userId);
    }
}
