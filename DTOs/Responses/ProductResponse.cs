using FoodFlowSystem.DTOs.Responses.Feedbacks;

namespace FoodFlowSystem.DTOs.Responses
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }
        public double AverageRated { get; set; }
        public CategoryResponse Category { get; set; }
        public ICollection<FeedbackResponse> Feedbacks { get; set; }
    }
}
