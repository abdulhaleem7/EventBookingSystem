using EventBookingSystem.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Repositories.Interfaces
{
    public interface IBookingRepository : IRepositoryAsync<Booking>
    {
        Task<IList<Booking>> GetByUserIdAsync(string userId);
    }
}
