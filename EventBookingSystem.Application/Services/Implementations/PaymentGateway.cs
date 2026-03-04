using EventBookingSystem.Application.Dtos;
using EventBookingSystem.Application.Services.Interfaces;
using EventBookingSystem.Infrastructure.Data;

namespace EventBookingSystem.Application.Services.Implementations
{
    public class PaymentGateway : IPaymentGateway
    {
        private static readonly Dictionary<string, bool> _payments = new();

        public Task<PaymentResult> InitializePayment(decimal amount, string reference)
        {
            var success = new Random().Next(1, 100) <= 95;

            _payments[reference] = success;

            return Task.FromResult(new PaymentResult
            {
                IsSuccessful = true,
                Reference = reference
            });
        }

        public Task<PaymentResult> VerifyPayment(string reference)
        {
            if (!_payments.ContainsKey(reference))
            {
                return Task.FromResult(new PaymentResult
                {
                    IsSuccessful = false,
                    Reference = reference
                });
            }

            return Task.FromResult(new PaymentResult
            {
                IsSuccessful = _payments[reference],
                Reference = reference
            });
        }
    }
}
