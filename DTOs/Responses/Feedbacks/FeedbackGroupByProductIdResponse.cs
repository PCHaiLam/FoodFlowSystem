namespace FoodFlowSystem.DTOs.Responses.Feedbacks
{
    public class FeedbackGroupByProductIdResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int TotalFeedbacks { get; set; }
        public double AverageRating { get; set; }
        public ICollection<FeedbackResponse> Feedbacks { get; set; }
    }
}
