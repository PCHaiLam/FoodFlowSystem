using FoodFlowSystem.DTOs.Responses.Feedbacks;

namespace FoodFlowSystem.DTOs.Responses.Recommendations
{
    public class ProductRecommendations
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public double AverageRate { get; set; }
        public int TotalFeedbacks { get; set; }
        public int TotalOrders { get; set; }
        public int TotalSales { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
    }
}
