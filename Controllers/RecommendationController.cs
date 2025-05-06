using FoodFlowSystem.Services.Recommendations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodFlowSystem.Controllers
{
    [Route("api/recommendations")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationsService _recommendationsService;

        public RecommendationController(IRecommendationsService recommendationsService)
        {
            _recommendationsService = recommendationsService;
        }

        [HttpGet("best-seller")]
        public async Task<IActionResult> GetBestSeller()
        {
            var result = await _recommendationsService.GetBestSellerAsync();
            return Ok(result);
        }

        [HttpGet("top-rated")]
        public async Task<IActionResult> GetTopRated()
        {
            var result = await _recommendationsService.GetTopRatedAsync();
            return Ok(result);
        }

        [HttpGet("personalized")]
        public async Task<IActionResult> GetPersonalizedRecommendations()
        {
            var result = await _recommendationsService.GetPersonalizedRecommendationsAsync();
            return Ok(result);
        }
    }
}
