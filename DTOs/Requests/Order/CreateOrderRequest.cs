using FoodFlowSystem.DTOs.Requests.OrderItem;

namespace FoodFlowSystem.DTOs.Requests.Order
{
    public class CreateOrderRequest
    {
        public int TableID { get; set; }
        public int ReservationID { get; set; }
        public ICollection<CreateOrderItemRequest> OrderItems { get; set; }
    }
}
