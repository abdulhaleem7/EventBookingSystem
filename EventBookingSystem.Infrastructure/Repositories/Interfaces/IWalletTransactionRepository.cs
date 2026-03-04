using EventBookingSystem.Domain.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Repositories.Interfaces
{
    public interface IWalletTransactionRepository : IRepositoryAsync<WalletTransaction>
    {
        Task<IList<WalletTransaction>> GetByWalletIdAsync(string walletId);
        Task<WalletTransaction> GetTransaction(Expression<Func<WalletTransaction, bool>> expression);
    }
}
