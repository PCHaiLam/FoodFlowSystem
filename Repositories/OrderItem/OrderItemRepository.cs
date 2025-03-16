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
            return await _dbContext.OrderItems.Where(x => x.OrderID == id).ToListAsync();
        }

        public async Task<OrderItemEntity> GetByOrderIdAndProductId(int orderId, int productId)
        {
            return await _dbContext.OrderItems.FirstOrDefaultAsync(x => x.OrderID == orderId && x.ProductID == productId);
        }
    }
}
