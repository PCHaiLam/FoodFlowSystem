using FoodFlowSystem.Entities.Feedback;

namespace FoodFlowSystem.Repositories.Feedback
{
    public interface IFeedbackRepository : IBaseRepository<FeedbackEntity>
    {
        Task<double> GetAverageRateByProductIdAsync(int id);
        Task<ICollection<FeedbackEntity>> GetByUserIdAsync(int id);
        Task<ICollection<FeedbackEntity>> GetByProductIdAsync(int id);
    }
}
