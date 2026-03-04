using EventBookingSystem.Domain.Models.Contracts;

namespace EventBookingSystem.Domain.Models
{
    public class Event : AuditableEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }

        public DateTime EventDate { get; set; }

        public decimal TicketPrice { get; set; }
        public int TotalTickets { get; set; }
        public int AvailableTickets { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
