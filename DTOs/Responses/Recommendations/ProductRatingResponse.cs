namespace FoodFlowSystem.DTOs.Responses.Recommendations
{
    public class ProductRatedResponse
    {
        public int ProductId { get; set; }
        public double AverageRated { get; set; }
        public int TotalFeedbacks { get; set; }
    }
}
