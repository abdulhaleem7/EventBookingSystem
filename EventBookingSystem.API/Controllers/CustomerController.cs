using Microsoft.AspNetCore.Mvc;
using EventBookingSystem.Application.Services.Interfaces;
using EventBookingSystem.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EventBookingSystem.Domain.Constant;

namespace EventBookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController(ICustomerService customerService) : ControllerBase
{
    private readonly ICustomerService _customerService = customerService;

    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var result = await _customerService.CreateUserAsync(request);
        if (result.Success)
        {
            return CreatedAtAction(null, result.Data);
        }

        return BadRequest(result.Message);
    }

    [HttpGet("GetUserProfile")]
    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> GetUserProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("UnAuthorize");

        var res = await _customerService.GetCustomerDetailsAsync(userId);
        if (res.Success) return Ok(res.Data);
        return StatusCode(res.StatusCode == 0 ? 400 : res.StatusCode, res.Message);
    }

}
