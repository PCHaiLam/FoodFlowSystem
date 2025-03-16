using FoodFlowSystem.DTOs.Requests.OrderItem;

namespace FoodFlowSystem.DTOs.Requests.Order
{
    public class UpdateOrderRequest
    {
        public int OrderID { get; set; }
        public ICollection<UpdateOrderItemRequest> OrderItems { get; set; }
    }
}
