using FoodFlowSystem.DTOs.Responses.Feedbacks;

namespace FoodFlowSystem.DTOs.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Note { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string PaymentUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserResponse User { get; set; }
        public ICollection<OrderItemResponse> OrderItems { get; set; }
    }
}
