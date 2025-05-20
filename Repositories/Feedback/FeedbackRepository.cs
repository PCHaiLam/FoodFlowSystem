using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.DTOs.Responses.Feedbacks;
using FoodFlowSystem.DTOs.Responses.Recommendations;
using FoodFlowSystem.Entities.Feedback;
using Microsoft.EntityFrameworkCore;

namespace FoodFlowSystem.Repositories.Feedback
{
    public class FeedbackRepository : BaseRepository<FeedbackEntity>, IFeedbackRepository
    {
        public FeedbackRepository(MssqlDbContext context) : base(context)
        {
        }

        public async Task AddListFeedbacksAsync(ICollection<FeedbackEntity> feedbacks)
        {
            await _dbContext.Feedbacks.AddRangeAsync(feedbacks);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ProductRatedResponse> GetAverageRateAndTotalFeedbacksByProductIdAsync(int productId)
        {
            var data = await _dbContext.Feedbacks
                .Where(x => x.ProductID == productId)
                .GroupBy(x => x.ProductID)
                .Select(g => new ProductRatedResponse
                {
                    ProductId = g.Key,
                    ProductName = g.Select(x => x.Product.Name).FirstOrDefault(),
                    AverageRated = g.Average(x => x.Rating),
                    TotalFeedbacks = g.Count()
                })
                .FirstOrDefaultAsync();

            return data;
        }

        public async Task<ICollection<FeedbackEntity>> GetByProductIdAsync(int id)
        {
            return await _dbContext.Feedbacks.Where(x => x.ProductID == id).ToListAsync();
        }

        public async Task<ICollection<FeedbackEntity>> GetByUserIdAsync(int id)
        {
            return await _dbContext.Feedbacks.Where(x => x.UserID == id).ToListAsync();
        }

        public async Task<ICollection<PendingFeedbackResponse>> GetPendingFeedbackByUserIdAsync(int userId)
        {
            var latestOrder = await _dbContext.Orders
                .Where(o => o.Status == "Completed" && o.UserID == userId)
                .OrderByDescending(o => o.ID)
                .FirstOrDefaultAsync();

            if (latestOrder == null)
            {
                return new List<PendingFeedbackResponse>();
            }

            var hasFeedback = await _dbContext.Feedbacks
                .AnyAsync(f => f.OrderID == latestOrder.ID && f.UserID == userId);

            if (hasFeedback)
            {
                return new List<PendingFeedbackResponse>();
            }

            var result = await _dbContext.OrderItems
                .Where(oi => oi.OrderID == latestOrder.ID)
                .Join(_dbContext.Products,
                    oi => oi.ProductID,
                    p => p.ID,
                    (oi, p) => new PendingFeedbackResponse
                    {
                        OrderId = latestOrder.ID,
                        ProductId = p.ID,
                        ProductName = p.Name,
                        PaymentDate = latestOrder.UpdatedAt
                    })
                .ToListAsync();

            return result;
        }

        public async Task<ICollection<ProductRecommendations>> GetTopRatedAsync()
        {
            var data = await _dbContext.Feedbacks
                .Include(x => x.Product)
                .GroupBy(x => x.ProductID)
                .Select(g => new ProductRecommendations
                {
                    ProductId = g.Key,
                    ProductName = g.Select(x => x.Product.Name).FirstOrDefault(),
                    ImageUrl = g.Select(x => x.Product.ImageUrl).FirstOrDefault(),
                    AverageRate = g.Average(x => x.Rating),
                    TotalFeedbacks = g.Count(),
                    Price = g.Select(x => x.Product.ProductVersions.OrderByDescending(pv => pv.ID).FirstOrDefault().Price).FirstOrDefault(),
                    CategoryName = g.Select(x => x.Product.Category.Name).FirstOrDefault()
                })
                .OrderByDescending(x => x.AverageRate)
                .ToListAsync();

            return data;
        }
    }
}
