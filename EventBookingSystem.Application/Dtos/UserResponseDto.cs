namespace EventBookingSystem.Application.Dtos
{
    public class UserResponseDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string? FullName { get; set; }
        public decimal WalletBalance { get; set; }
    }
}
