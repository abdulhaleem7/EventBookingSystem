using EventBookingSystem.Domain.Enums;
using EventBookingSystem.Domain.Models.Contracts;

namespace EventBookingSystem.Domain.Models
{
    public class Booking : AuditableEntity
    {

        public string? UserId { get; set; }
        public User? User { get; set; }
        public string? EventId { get; set; }
        public Event? Event { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public BookingStatus Status { get; set; }

    }
}
