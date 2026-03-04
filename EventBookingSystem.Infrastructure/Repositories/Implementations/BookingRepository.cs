using EventBookingSystem.Domain.Models;
using EventBookingSystem.Infrastructure.Data;
using EventBookingSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Repositories.Implementations
{
    public class BookingRepository(AppDbContext context) : RepositoryAsync<Booking>(context), IBookingRepository
    {
        public async Task<IList<Booking>> GetByUserIdAsync(string userId)
        {
            return await DbSet.Where(b => b.UserId == userId).ToListAsync();
        }
    }
}
