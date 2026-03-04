using EventBookingSystem.Domain.Models;
using EventBookingSystem.Infrastructure.Data;
using EventBookingSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Repositories.Implementations
{
    public class WalletTransactionRepository(AppDbContext context) : RepositoryAsync<WalletTransaction>(context), IWalletTransactionRepository
    {
        public async Task<IList<WalletTransaction>> GetByWalletIdAsync(string walletId)
        {
            return await context.WalletTransactions.Where(wt => wt.WalletId == walletId).ToListAsync();
        }

        public async Task<WalletTransaction> GetTransaction(Expression<Func<WalletTransaction, bool>> expression)
        {
            return await context.WalletTransactions.Include(x => x.Wallet).FirstOrDefaultAsync(expression);
        }
    }
}
