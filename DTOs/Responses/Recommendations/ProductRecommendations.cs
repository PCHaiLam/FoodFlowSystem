using FoodFlowSystem.DTOs.Responses.Feedbacks;

namespace FoodFlowSystem.DTOs.Responses.Recommendations
{
    public class ProductRecommendations
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public double AverageRated { get; set; }
        public int TotalFeedbacks { get; set; }
        public int TotalOrders { get; set; }
        public int TotalSales { get; set; }
        public string CategoryName { get; set; }
    }
}
