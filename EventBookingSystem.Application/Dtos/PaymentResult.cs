namespace EventBookingSystem.Application.Dtos
{
    public class PaymentResult
    {
        public bool IsSuccessful { get; set; }
        public string? Reference { get; set; }
    }
}
