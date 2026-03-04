using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EventBookingSystem.Application.Services.Interfaces;
using EventBookingSystem.Application.Dtos;

namespace EventBookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    /// <summary>
    /// Customer login - returns JWT on success
    /// </summary>
    /// <remarks>
    /// Seeded test credentials (one example):
    ///
    /// {
    ///   "email": "abdul.salaudeen@gmail.com",
    ///   "password": "StrongPass123!"
    /// }
    ///
    /// Use the above JSON as the request body in Swagger's "Try it out" panel.
    /// </remarks>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var result = await _authenticationService.CustomerLoginAsync(request);
        if (result.Success)
            return Ok(result.Data);

        return StatusCode(result.StatusCode == 0 ? 400 : result.StatusCode, result.Message);
    }

    /// <summary>
    /// Admin login - returns JWT for admin accounts
    /// </summary>
    /// <remarks>
    /// Seeded admin credentials (one example):
    ///
    /// {
    ///   "email": "admin@eventhub.com",
    ///   "password": "AdminSecure123!"
    /// }
    ///
    /// Use the above JSON as the request body in Swagger's "Try it out" panel.
    /// </remarks>
    [HttpPost("AdminLogin")]
    [AllowAnonymous]
    public async Task<IActionResult> AdminLogin([FromBody] LoginUserRequest request)
    {
        var result = await _authenticationService.AdminLoginAsync(request);
        if (result.Success)
            return Ok(result.Data);

        return StatusCode(result.StatusCode == 0 ? 400 : result.StatusCode, result.Message);
    }
}
