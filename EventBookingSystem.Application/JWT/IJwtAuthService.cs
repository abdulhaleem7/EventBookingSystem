using System.Security.Claims;

namespace EventBookingSystem.Application.JWT
{
    public interface IJwtAuthService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
    }
}
