using EventBookingSystem.Application.Dtos;

namespace EventBookingSystem.Application.Services.Interfaces
{
    public interface IPaymentGateway
    {
        Task<PaymentResult> InitializePayment(decimal amount, string reference);
        Task<PaymentResult> VerifyPayment(string reference);
    }
}
