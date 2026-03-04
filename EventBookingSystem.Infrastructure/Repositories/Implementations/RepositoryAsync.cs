using EventBookingSystem.Infrastructure.Data;
using EventBookingSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Repositories.Implementations
{
    public class RepositoryAsync<T>(AppDbContext context) : IRepositoryAsync<T> where T : class, new()
    {
        private readonly AppDbContext _context = context;
        internal DbSet<T> DbSet = context.Set<T>();

        public async Task AddAsync(T entity) => await DbSet.AddAsync(entity);

        public async Task<bool> Exist(Expression<Func<T, bool>> expression) => await DbSet.AnyAsync(expression);

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression) => await DbSet.FirstOrDefaultAsync(expression);

        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> expression = null)
        {
            if (expression == null)
                return await DbSet.ToListAsync();

            return await DbSet.Where(expression).ToListAsync();
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
