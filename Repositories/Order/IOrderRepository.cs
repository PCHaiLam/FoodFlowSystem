using FoodFlowSystem.Entities.Order;

namespace FoodFlowSystem.Repositories.Order
{
    public interface IOrderRepository : IBaseRepository<OrderEntity>
    {
        Task<OrderEntity> IsExistOrderAsync(int id);
        Task<IEnumerable<OrderEntity>> GetByDateAsync(DateTime date);
        Task<IEnumerable<OrderEntity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<OrderEntity>> GetByUserIdAsync(int id);
    }
}
