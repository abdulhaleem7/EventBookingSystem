namespace EventBookingSystem.Application.Dtos
{
    public class EventResponseDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        public decimal TicketPrice { get; set; }
        public int TotalTickets { get; set; }
        public int AvailableTickets { get; set; }
    }
}
