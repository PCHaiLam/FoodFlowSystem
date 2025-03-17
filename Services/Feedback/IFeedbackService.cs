using FoodFlowSystem.DTOs.Requests.Feedback;
using FoodFlowSystem.DTOs.Responses;

namespace FoodFlowSystem.Services.Feedback
{
    public interface IFeedbackService
    {
        Task<FeedbackResponse> GetFeedbackAsync(int id);
        Task<ICollection<FeedbackResponse>> GetAllFeedbacksAsync();
        Task<ICollection<FeedbackResponse>> GetFeedbacksByUserIdAsync(int id);
        Task<ICollection<FeedbackResponse>> GetFeedbacksByProductIdAsync(int id);
        Task<FeedbackResponse> CreateFeedbackAsync(CreateFeedbackRequest request);
        Task<FeedbackResponse> UpdateFeedbackAsync(UpdateFeedbackRequest request);
        Task DeleteFeedbackAsync(int id);
    }
}
