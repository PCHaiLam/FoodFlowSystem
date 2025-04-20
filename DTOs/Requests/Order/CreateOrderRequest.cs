using FoodFlowSystem.DTOs.Requests.OrderItem;

namespace FoodFlowSystem.DTOs.Requests.Order
{
    public class CreateOrderRequest
    {
        public string Phone { get; set; }
        public int? TableId { get; set; }
        public int? NumOfGuests { get; set; }
        public TimeOnly? ReservationTime { get; set; }
        public DateTime? ReservationDate { get; set; }
        public string Note { get; set; }
        public int Discount { get; set; }
        public ICollection<CreateOrderItemRequest> OrderItems { get; set; }
    }
}
