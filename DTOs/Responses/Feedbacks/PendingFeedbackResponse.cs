namespace FoodFlowSystem.DTOs.Responses.Feedbacks
{
    public class PendingFeedbackResponse
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
