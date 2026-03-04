using EventBookingSystem.Domain.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBookingSystem.Domain.Models
{
    public class Admin : AuditableEntity
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
