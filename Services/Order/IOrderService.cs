using FoodFlowSystem.DTOs.Requests.Order;
using FoodFlowSystem.DTOs.Responses;

namespace FoodFlowSystem.Services.Order
{
    public interface IOrderService
    {
        Task<ICollection<OrderResponse>> GetAllOrdersAsync(int page, int pageSize);
        Task<ICollection<OrderResponse>> GetOrdersByUserId(int id);
        Task<ICollection<OrderResponse>> GetPendingOrdersAsync();
        Task<OrderResponse> GetOrderById(int id);
        Task<ICollection<OrderResponse>> GetOrderByDate(DateTime date);
        Task<ICollection<OrderResponse>> GetOrderRangeDate(DateTime startDate, DateTime endDate);
        Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request);
        Task<OrderResponse> UpdateOrderAsync(UpdateOrderRequest request);
        Task DeleteOrderAsync(int id);
    }
}
