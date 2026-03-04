using EventBookingSystem.Domain.Models;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Repositories.Interfaces
{
    public interface IWalletRepository : IRepositoryAsync<Wallet>
    {
        Task<Wallet> GetByUserIdAsync(string userId);
    }
}
