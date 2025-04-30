using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities.OrderItem;
using Microsoft.EntityFrameworkCore;

namespace FoodFlowSystem.Repositories.OrderItem
{
    public class OrderItemRepository : BaseRepository<OrderItemEntity>, IOrderItemRepository
    {
        public OrderItemRepository(MssqlDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ICollection<OrderItemEntity>> GetByOrderId(int id)
        {
            var result = await _dbContext.OrderItems
               .Include(x => x.Product)
               .Where(x => x.OrderID == id)
               .ToListAsync();
            return result;
        }

        public async Task<OrderItemEntity> GetByOrderIdAndProductId(int orderId, int productId)
        {
            return await _dbContext.OrderItems.FirstOrDefaultAsync(x => x.OrderID == orderId && x.ProductID == productId);
        }
    }
}
