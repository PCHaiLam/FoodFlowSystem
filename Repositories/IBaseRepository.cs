using FoodFlowSystem.Entities;

namespace FoodFlowSystem.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync(int page = 1, int pageSize = 10);
        Task<int> CountAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> AddWithoutSavingAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(int id);
        T UpdateWithoutSaving(T entity);
    }
}
