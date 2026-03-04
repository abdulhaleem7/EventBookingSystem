using EventBookingSystem.Domain.Models;
using EventBookingSystem.Infrastructure.Data;
using EventBookingSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Repositories.Implementations
{
    public class WalletRepository(AppDbContext context) : RepositoryAsync<Wallet>(context), IWalletRepository
    {
        public async Task<Wallet> GetByUserIdAsync(string userId)
        {
            return await DbSet.Include(w => w.Transactions).FirstOrDefaultAsync(w => w.UserId == userId);
        }
    }
}
