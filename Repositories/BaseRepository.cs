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

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return null;
            }

            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}