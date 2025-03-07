using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodFlowSystem.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly MssqlDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(MssqlDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<T> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
