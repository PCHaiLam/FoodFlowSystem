using FoodFlowSystem.DTOs.Requests.OrderItem;

namespace FoodFlowSystem.DTOs.Requests.Order
{
    public class CreateOrderRequest
    {
        public int? TableID { get; set; }
        public int? NumOfGuests { get; set; }
        //public bool? HasFoodOrder { get; set; }
        //public bool? HasReservation { get; set; }
        public TimeOnly? ReservationTime { get; set; }
        public DateTime? ReservationDate { get; set; }
        public string Notes { get; set; }
        public ICollection<CreateOrderItemRequest> OrderItems { get; set; }
    }
}
