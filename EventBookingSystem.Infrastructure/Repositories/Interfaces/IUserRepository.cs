using EventBookingSystem.Domain.Models;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository : IRepositoryAsync<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
