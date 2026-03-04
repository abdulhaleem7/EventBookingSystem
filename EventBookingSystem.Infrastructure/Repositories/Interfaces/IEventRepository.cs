using EventBookingSystem.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Repositories.Interfaces
{
    public interface IEventRepository : IRepositoryAsync<Event>
    {
        Task<IList<Event>> GetAvailableEventsAsync();
    }
}
