using FoodFlowSystem.DTOs.Responses.Recommendations;
using FoodFlowSystem.DTOs.Responses.Statistic;
using FoodFlowSystem.Entities.OrderItem;

namespace FoodFlowSystem.Repositories.OrderItem
{
    public interface IOrderItemRepository : IBaseRepository<OrderItemEntity>
    {
        Task<ICollection<OrderItemEntity>> GetByOrderId(int id);
        Task<OrderItemEntity> GetByOrderIdAndProductId(int orderId, int productId);
        Task<ProductSalesResponse> GetTotalOrdersAndTotalSalesByProductIdAsync(int productId);
        Task<ICollection<ProductStatisticResponse>> GetByArangeDateAsync(DateTime startDate, DateTime endDate);
    }
}
