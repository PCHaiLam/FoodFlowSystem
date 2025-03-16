using FoodFlowSystem.Entities.OrderItem;

namespace FoodFlowSystem.Repositories.OrderItem
{
    public interface IOrderItemRepository : IBaseRepository<OrderItemEntity>
    {
        Task<ICollection<OrderItemEntity>> GetByOrderId(int id);
        Task<OrderItemEntity> GetByOrderIdAndProductId(int orderId, int productId);
    }
}
