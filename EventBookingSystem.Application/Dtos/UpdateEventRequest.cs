namespace EventBookingSystem.Application.Dtos
{
    public class UpdateEventRequest
    {
        public string EventId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        public decimal TicketPrice { get; set; }
        public int TotalTickets { get; set; }
    }
}
