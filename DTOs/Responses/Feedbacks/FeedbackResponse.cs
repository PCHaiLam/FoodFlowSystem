namespace FoodFlowSystem.DTOs.Responses.Feedbacks
{
    public class FeedbackResponse
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserResponse User { get; set; }
    }
}
