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

        public async Task<IEnumerable<OrderEntity>> GetAllOrdersAsync(int page, int size, string search = null)
        {
            var query = _dbContext.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.User.LastName.Contains(search) || x.User.Email.Contains(search));
            }

            var data = await query
                .Include(x => x.User)
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return data;
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

        public async Task<OrderEntity> GetOrderDetailByIdAsync(int id)
        {
            var data = await _dbContext.Orders
                .Include(x => x.User)
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.ID == id);
            return data;
        }

        public async Task<IEnumerable<OrderEntity>> GetPendingOrdersAsync()
        {
            var data = await _dbContext.Orders
                .Include(x => x.User)
                .Where(x => x.Status == "Pending")
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
            return data;
        }

        public async Task<OrderEntity> IsExistOrderAsync(int id)
        {
            var data = await _dbContext.Orders.FirstOrDefaultAsync(x => x.ID == id);
            return data;
        }
    }
}
