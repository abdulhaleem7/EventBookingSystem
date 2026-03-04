using EventBookingSystem.Domain.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using EventBookingSystem.Domain.Constant;

namespace EventBookingSystem.Domain.Models
{
    public class User : AuditableEntity
    {
        public string? Email { get; set; }
        // Stored password hash (do not store plain text passwords)
        public string? PasswordHash { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public Wallet? Wallet { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
