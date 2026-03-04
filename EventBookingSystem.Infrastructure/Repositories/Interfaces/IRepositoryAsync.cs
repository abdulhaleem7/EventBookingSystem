using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Repositories.Interfaces
{
    public interface IRepositoryAsync<T> where T : class
    {
        Task AddAsync(T entity);
        Task<bool> Exist(Expression<Func<T, bool>> expression);
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> expression = null);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        Task<int> SaveChangesAsync();
    }
}
