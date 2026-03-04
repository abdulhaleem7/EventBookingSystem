using EventBookingSystem.Application.Dtos;
using EventBookingSystem.Application.Services.Interfaces;
using EventBookingSystem.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventBookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(IEventService eventService) : ControllerBase
{
    private readonly IEventService _eventService = eventService;

    [HttpGet("GetAllEvent")]
    public async Task<IActionResult> GetAllEvent()
    {
        var res = await _eventService.GetAllAsync();
        return Ok(res.Data);
    }

    [HttpGet("GetAvailableEvents")]
    public async Task<IActionResult> GetAvailableEvents()
    {
        var res = await _eventService.GetAvailableEventsAsync();
        return Ok(res.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var res = await _eventService.GetByIdAsync(id);
        if (!res.Success) return NotFound(res.Message);
        return Ok(res.Data);
    }

    [HttpPost("CreateEvent")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
    {
        var res = await _eventService.CreateEventAsync(request);
        if (!res.Success) return BadRequest(res.Message);
        return CreatedAtAction(nameof(GetById), new { id = res.Data.Id }, res.Data);
    }

    [HttpPost("BookEvent")]
    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> BookEvent(CreateBookingRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("UnAuthorize");
        var result = await _eventService.BookTicketAsync(request, userId);

        return Ok(result);
    }

    [HttpPost("CancelBooking")]
    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> CancelBooking(string bookingId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("UnAuthorize");
        var result = await _eventService.CancelBookingAsync(bookingId, userId);

        return Ok(result);
    }


    [HttpPost("UpdateEvent")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventRequest request)
    {
        var res = await _eventService.UpdateEventAsync(request);
        if (!res.Success) return NotFound(res.Message);
        return Ok(res.Data);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> RemoveEvent(string id)
    {
        var res = await _eventService.DeleteEventAsync(id);
        if (!res.Success) return NotFound(res.Message);
        return NoContent();
    }
}
