using EventBookingSystem.Domain.Models;
using EventBookingSystem.Infrastructure.Data;
using EventBookingSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Repositories.Implementations
{
    public class EventRepository(AppDbContext context) : RepositoryAsync<Event>(context), IEventRepository
    {
        public async Task<IList<Event>> GetAvailableEventsAsync()
        {
            return await context.Events.Where(e => e.AvailableTickets > 0 && e.EventDate > DateTime.UtcNow)
                .ToListAsync();
        }
        
    }
}
