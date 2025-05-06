using FoodFlowSystem.DTOs.Responses.Recommendations;

namespace FoodFlowSystem.Services.Recommendations
{
    public interface IRecommendationsService
    {
        Task<ICollection<ProductRecommendations>> GetBestSellerAsync();
        Task<ICollection<ProductRecommendations>> GetTopRatedAsync();
        Task<ICollection<ProductRecommendations>> GetPersonalizedRecommendationsAsync();
    }
}
