using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.DTOs.Responses.Recommendations;
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

        public async Task<ProductSalesResponse> GetTotalOrdersAndTotalSalesByProductIdAsync(int productId)
        {
            var data = await _dbContext.OrderItems
                .Where(x => x.ProductID == productId)
                .GroupBy(x => x.ProductID)
                .Select(g => new ProductSalesResponse
                {
                    ProductId = g.Key,
                    TotalOrders = g.Count(),
                    TotalSales = g.Sum(x => x.Quantity)
                })
                .FirstOrDefaultAsync();

            return data;
        }
    }
}
