namespace FoodFlowSystem.DTOs.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Note { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<OrderItemResponse> OrderItems { get; set; }
    }
}
