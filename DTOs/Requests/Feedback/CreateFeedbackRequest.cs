namespace FoodFlowSystem.DTOs.Requests.Feedback
{
    public class CreateFeedbackRequest
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
