using Microsoft.Extensions.DependencyInjection;
using EventBookingSystem.Application.Services.Interfaces;
using EventBookingSystem.Application.Services.Implementations;
using EventBookingSystem.Application.JWT;

namespace EventBookingSystem.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IJwtAuthService, JwtAuthService>();
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<IPaymentGateway, PaymentGateway>();
        services.AddScoped<IJwtAuthService, JwtAuthService>();
        return services;
    }
}
