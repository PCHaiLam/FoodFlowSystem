using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities.Order;
using Microsoft.EntityFrameworkCore;

namespace FoodFlowSystem.Repositories.Order
{
    public class OrderRepository : BaseRepository<OrderEntity>, IOrderRepository
    {
        public OrderRepository(MssqlDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OrderEntity>> GetByDateAsync(DateTime date)
        {
            var data = await _dbContext.Orders.Where(x => x.CreatedAt == date).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<OrderEntity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var data = await _dbContext.Orders.Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<OrderEntity>> GetByUserIdAsync(int id)
        {
            var data = await _dbContext.Orders.Where(x => x.UserID == id).ToListAsync();
            return data;
        }

        public async Task<OrderEntity> IsExistOrderAsync(int id)
        {
            var data = await _dbContext.Orders.FirstOrDefaultAsync(x => x.ID == id);
            return data;
        }
    }
}
