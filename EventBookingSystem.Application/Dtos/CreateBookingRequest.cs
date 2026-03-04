using System;
using System.Collections.Generic;
using System.Text;

namespace EventBookingSystem.Application.Dtos
{
    public class CreateBookingRequest
    {
        public string? EventId { get; set; }
        public int Quantity { get; set; }
    }
}
