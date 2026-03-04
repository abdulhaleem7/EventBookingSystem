using EventBookingSystem.Application.Dtos;
using EventBookingSystem.Application.Services.Interfaces;
using EventBookingSystem.Domain.Enums;
using EventBookingSystem.Domain.Models;
using EventBookingSystem.Infrastructure.Repositories.Implementations;
using EventBookingSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventBookingSystem.Application.Services.Implementations;

public class EventService(IEventRepository eventRepository,
    IBookingRepository bookingRepository,
    IWalletRepository walletRepository,
    IWalletTransactionRepository walletTransactionRepository) : IEventService
{
    private readonly IEventRepository _eventRepository = eventRepository;
    private readonly IBookingRepository _bookingRepository = bookingRepository;
    private readonly IWalletRepository _walletRepository = walletRepository;
    private readonly IWalletTransactionRepository _walletTransactionRepository = walletTransactionRepository;

    public async Task<ApiResponse<EventResponseDto>> CreateEventAsync(CreateEventRequest request)
    {
        var ev = new Event
        {
            Title = request.Title,
            Description = request.Description,
            EventDate = request.EventDate,
            TicketPrice = request.TicketPrice,
            TotalTickets = request.TotalTickets,
            AvailableTickets = request.TotalTickets
        };

        await _eventRepository.AddAsync(ev);
        await _eventRepository.SaveChangesAsync();

        var dto = Map(ev);
        return ApiResponse<EventResponseDto>.Ok(dto);
    }

    public async Task<ApiResponse<string>> BookTicketAsync(CreateBookingRequest request, string userId)
    {
        var @event = await _eventRepository.GetAsync(x => x.Id == request.EventId && x.AvailableTickets > 0 && x.EventDate > DateTime.UtcNow);

        if (@event == null)
            return ApiResponse<string>.BadRequest("Event not found");

        if (@event.AvailableTickets < request.Quantity)
            return ApiResponse<string>.BadRequest("Not enough tickets available");
        var wallet = await _walletRepository.GetAsync(x => x.UserId == userId);

        if (wallet == null)
            return ApiResponse<string>.BadRequest("Wallet not found");
        var totalCost = @event.TicketPrice * request.Quantity;

        if (wallet.Balance < totalCost)
            return ApiResponse<string>.BadRequest("Insufficient wallet balance");

        wallet.Balance -= totalCost;

        @event.AvailableTickets -= request.Quantity;

        var booking = new Booking
        {
            EventId = request.EventId,
            UserId = userId,
            Quantity = request.Quantity
        };

        await _bookingRepository.AddAsync(booking);

        var walletTransaction = new WalletTransaction
        {
            WalletId = wallet.Id,
            Amount = totalCost,
            Type = TransactionType.Debit,
            Reference = $"BOOK-{Guid.NewGuid()}",
            Status = TransactionStatus.Completed
        };

        await _walletTransactionRepository.AddAsync(walletTransaction);
        await _eventRepository.SaveChangesAsync();

        return ApiResponse<string>.Ok("Booking successful");
    }
    public async Task<ApiResponse<IList<EventResponseDto>>> GetAllAsync()
    {
        var list = await _eventRepository.GetAllAsync();
        var dtos = list.Select(Map).ToList();
        return ApiResponse<IList<EventResponseDto>>.Ok(dtos);
    }
    public async Task<ApiResponse<IList<EventResponseDto>>> GetAvailableEventsAsync()
    {
        var events = await _eventRepository.GetAvailableEventsAsync();

        var dtos = events.Select(Map).ToList();

        return ApiResponse<IList<EventResponseDto>>.Ok(dtos);
    }
    public async Task<ApiResponse<EventResponseDto>> GetByIdAsync(string id)
    {
        var ev = await _eventRepository.GetAsync(e => e.Id == id);
        if (ev == null) return ApiResponse<EventResponseDto>.NotFound();
        return ApiResponse<EventResponseDto>.Ok(Map(ev));
    }

    public async Task<ApiResponse<string>> CancelBookingAsync(string bookingId,string userId)
    {
        var booking = await _bookingRepository.GetAsync(x => x.Id == bookingId && x.UserId == userId);

        if (booking == null)
            return ApiResponse<string>.BadRequest("Booking not found");

        if (booking.Status == BookingStatus.Cancelled)
            return ApiResponse<string>.BadRequest("Booking already cancelled");

        var @event = await _eventRepository.GetAsync(x => x.Id == booking.EventId);

        if (@event == null)
            return ApiResponse<string>.BadRequest("Event not found");

        if (@event.EventDate.Date <= DateTime.UtcNow.Date)
            return ApiResponse<string>.BadRequest("Booking cannot be cancelled on or after the event day");

        var wallet = await _walletRepository.GetAsync(x => x.UserId == booking.UserId);

        if (wallet == null)
            return ApiResponse<string>.BadRequest("Wallet not found");


        var refundAmount = @event.TicketPrice * booking.Quantity;

        @event.AvailableTickets += booking.Quantity;

        wallet.Balance += refundAmount;

        booking.Status = BookingStatus.Cancelled;

        var walletTransaction = new WalletTransaction
        {
            WalletId = wallet.Id,
            Amount = refundAmount,
            Type = TransactionType.Credit,
            Reference = $"REFUND-{Guid.NewGuid()}",
            Status = TransactionStatus.Completed,
        };

        await _walletTransactionRepository.AddAsync(walletTransaction);


        await _walletRepository.SaveChangesAsync();


        return ApiResponse<string>.Ok("Booking cancelled and wallet refunded successfully");

    }

    public async Task<ApiResponse<EventResponseDto>> UpdateEventAsync(UpdateEventRequest request)
    {
        var ev = await _eventRepository.GetAsync(e => e.Id == request.EventId);
        if (ev == null) return ApiResponse<EventResponseDto>.NotFound();

        ev.Title = request.Title;
        ev.Description = request.Description;
        ev.EventDate = request.EventDate;
        var diff = request.TotalTickets - ev.TotalTickets;
        ev.TotalTickets = request.TotalTickets;
        ev.AvailableTickets = ev.AvailableTickets + diff;
        ev.TicketPrice = request.TicketPrice;

        await _eventRepository.SaveChangesAsync();
        return ApiResponse<EventResponseDto>.Ok(Map(ev));
    }

    public async Task<ApiResponse<bool>> DeleteEventAsync(string id)
    {
        var ev = await _eventRepository.GetAsync(e => e.Id == id);
        if (ev == null) return ApiResponse<bool>.NotFound();
        ev.IsDeleted = false;
        await _eventRepository.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true);
    }

    private static EventResponseDto Map(Event ev)
    {
        return new EventResponseDto
        {
            Id = ev.Id,
            Title = ev.Title,
            Description = ev.Description,
            EventDate = ev.EventDate,
            TicketPrice = ev.TicketPrice,
            TotalTickets = ev.TotalTickets,
            AvailableTickets = ev.AvailableTickets
        };
    }

}
