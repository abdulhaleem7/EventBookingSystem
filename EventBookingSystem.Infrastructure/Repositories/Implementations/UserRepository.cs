using EventBookingSystem.Domain.Models;
using EventBookingSystem.Infrastructure.Data;
using EventBookingSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Repositories.Implementations
{
    public class UserRepository(AppDbContext context) : RepositoryAsync<User>(context), IUserRepository
    {
        public async Task<User> GetByEmailAsync(string email)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
