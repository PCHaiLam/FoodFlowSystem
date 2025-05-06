using FoodFlowSystem.DTOs.Responses.Feedbacks;
using FoodFlowSystem.DTOs.Responses.Recommendations;
using FoodFlowSystem.Entities.Feedback;
using FoodFlowSystem.Repositories.Product;

namespace FoodFlowSystem.Repositories.Feedback
{
    public interface IFeedbackRepository : IBaseRepository<FeedbackEntity>
    {
        Task AddListFeedbacksAsync(ICollection<FeedbackEntity> feedbacks);
        Task<ProductRatedResponse> GetAverageRateAndTotalFeedbacksByProductIdAsync(int productId);
        Task<ICollection<FeedbackEntity>> GetByUserIdAsync(int id);
        Task<ICollection<FeedbackEntity>> GetByProductIdAsync(int id);
        Task<ICollection<PendingFeedbackResponse>> GetPendingFeedbackByUserIdAsync(int userId);
        Task<ICollection<ProductRecommendations>> GetTopRatedAsync();
    }
}
