using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities.Feedback;
using Microsoft.EntityFrameworkCore;

namespace FoodFlowSystem.Repositories.Feedback
{
    public class FeedbackRepository : BaseRepository<FeedbackEntity>, IFeedbackRepository
    {
        public FeedbackRepository(MssqlDbContext context) : base(context)
        {
        }

        public async Task<ICollection<FeedbackEntity>> GetByProductIdAsync(int id)
        {
            return await _dbContext.Feedbacks.Where(x => x.ProductID == id).ToListAsync();
        }

        public async Task<ICollection<FeedbackEntity>> GetByUserIdAsync(int id)
        {
            return await _dbContext.Feedbacks.Where(x => x.UserID == id).ToListAsync();
        }
    }
}
