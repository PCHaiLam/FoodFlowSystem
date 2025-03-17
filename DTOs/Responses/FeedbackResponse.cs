namespace FoodFlowSystem.DTOs.Responses
{
    public class FeedbackResponse
    {
        public int FeedbackId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
