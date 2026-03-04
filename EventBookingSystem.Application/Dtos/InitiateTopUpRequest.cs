namespace EventBookingSystem.Application.Dtos
{
    public class InitiateTopUpRequest
    {
        public decimal Amount { get; set; }
        public string? IdempotencyKey { get; set; }
    }
}
