namespace FoodFlowSystem.DTOs.Requests.Feedback
{
    public class CreatListFeedbacksRequest
    {
        public ICollection<CreateFeedbackRequest> ListFeedbacks { get; set; }
    }
}
