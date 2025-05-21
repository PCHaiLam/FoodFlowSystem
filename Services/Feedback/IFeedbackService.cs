using FoodFlowSystem.DTOs.Requests.Feedback;
using FoodFlowSystem.DTOs.Responses.Feedbacks;

namespace FoodFlowSystem.Services.Feedback
{
    public interface IFeedbackService
    {
        Task<FeedbackResponse> GetFeedbackAsync(int id);
        Task<ICollection<FeedbackResponse>> GetAllFeedbacksAsync(int top);
        Task<ICollection<FeedbackResponse>> GetFeedbacksByUserIdAsync(int id);
        Task<ICollection<PendingFeedbackResponse>> GetPendingFeedbackByUserId();
        Task<ICollection<FeedbackResponse>> GetFeedbacksByProductIdAsync(int id);
        Task<FeedbackResponse> CreateFeedbackAsync(CreateFeedbackRequest request);
        Task CreateListFeedbacksAsync(CreatListFeedbacksRequest requests);
        Task<FeedbackResponse> UpdateFeedbackAsync(UpdateFeedbackRequest request);
        Task DeleteFeedbackAsync(int id);
    }
}
