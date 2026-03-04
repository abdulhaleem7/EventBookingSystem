using Microsoft.AspNetCore.Mvc;
using EventBookingSystem.Application.Services.Interfaces;
using EventBookingSystem.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EventBookingSystem.Domain.Constant;

namespace EventBookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletsController(IWalletService walletService) : ControllerBase
{
    private readonly IWalletService _walletService = walletService;

    [HttpPost("InitiateTopUp")]
    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> InitiateTopUp([FromBody] InitiateTopUpRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User id not found in token.");


        var res = await _walletService.InitiateTopUpAsync(request, userId);
        if (res.Success) return Ok(res.Data);
        return StatusCode(res.StatusCode == 0 ? 400 : res.StatusCode, res.Message);
    }

    [HttpPost("ConfirmTopUp/{reference}")]
    [Authorize(Roles = Roles.Customer)]
    public async Task<IActionResult> ConfirmTopUp(string reference)
    {
        var res = await _walletService.ConfirmTopUpAsync(reference);
        if (res.Success) return Ok(res.Data);
        return StatusCode(res.StatusCode == 0 ? 400 : res.StatusCode, res.Message);
    }
}
