using AutoMapper;
using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Feedback;
using FoodFlowSystem.DTOs.Responses.Recommendations;
using FoodFlowSystem.Repositories.Feedback;
using FoodFlowSystem.Repositories.Order;
using FoodFlowSystem.Repositories.OrderItem;
using FoodFlowSystem.Services.Feedback;
using FoodFlowSystem.Validators.Feedback;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FoodFlowSystem.Services.Recommendations
{
    public class RecommendationsService : BaseService, IRecommendationsService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ILogger<FeedbackService> _logger;

        public RecommendationsService(
            IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IFeedbackRepository feedbackRepository,
            ILogger<FeedbackService> logger
            ) : base(httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _feedbackRepository = feedbackRepository;
            _logger = logger;
        }

        public async Task<ICollection<ProductRecommendations>> GetBestSellerAsync()
        {
            var data = await _orderRepository.GetBestSellerAsync();
            var result = new List<ProductRecommendations>();

            foreach (var item in data)
            {
                var feedback = await _feedbackRepository.GetAverageRateAndTotalFeedbacksByProductIdAsync(item.Id);
                result.Add(new ProductRecommendations
                {
                    Id = item.Id,
                    Name = item.Name,
                    ImageUrl = item.ImageUrl,
                    TotalOrders = item.TotalOrders,
                    TotalSales = item.TotalSales,
                    Price = item.Price,
                    CategoryName = item.CategoryName,
                    AverageRated = feedback?.AverageRated ?? 0,
                    TotalFeedbacks = feedback?.TotalFeedbacks ?? 0
                });
            }

            return result;
        }

        public async Task<ICollection<ProductRecommendations>> GetPersonalizedRecommendationsAsync()
        {
            var userId = this.GetCurrentUserId();
            var feedbacks = await _feedbackRepository.GetByUserIdAsync(userId);
            var productIds = feedbacks.Select(x => x.ProductID).ToList();

            var data = await _orderRepository.GetBestSellerAsync();
            var result = new List<ProductRecommendations>();

            foreach (var item in data)
            {
                if (!productIds.Contains(item.Id))
                {
                    var feedback = await _feedbackRepository.GetAverageRateAndTotalFeedbacksByProductIdAsync(item.Id);
                    result.Add(new ProductRecommendations
                    {
                        Id = item.Id,
                        Name = item.Name,
                        ImageUrl = item.ImageUrl,
                        TotalOrders = item.TotalOrders,
                        TotalSales = item.TotalSales,
                        Price = item.Price,
                        CategoryName = item.CategoryName,
                        AverageRated = feedback?.AverageRated ?? 0,
                        TotalFeedbacks = feedback?.TotalFeedbacks ?? 0
                    });
                }
            }

            return result;
        }

        public async Task<ICollection<ProductRecommendations>> GetTopRatedAsync()
        {
            var data = await _feedbackRepository.GetTopRatedAsync();
            var result = new List<ProductRecommendations>();

            foreach (var item in data)
            {
                var orderItem = await _orderItemRepository.GetTotalOrdersAndTotalSalesByProductIdAsync(item.Id);

                result.Add(new ProductRecommendations
                {
                    Id = item.Id,
                    Name = item.Name,
                    ImageUrl = item.ImageUrl,
                    AverageRated = item.AverageRated,
                    TotalFeedbacks = item.TotalFeedbacks,
                    TotalOrders = orderItem?.TotalOrders ?? 0,
                    TotalSales = orderItem?.TotalSales ?? 0,
                    Price = item.Price,
                    CategoryName = item.CategoryName
                });
            }

            return result;
        }
    }
}
