using FoodFlowSystem.Entities.Category;
using FoodFlowSystem.Entities.Feedback;
using FoodFlowSystem.Entities.OrderItem;
using FoodFlowSystem.Entities.ProductVersions;
using FoodFlowSystem.Entities.UserRecommendations;

namespace FoodFlowSystem.Entities.Product
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int? CategoryID { get; set; }
        public CategoryEntity Category { get; set; }
        public ICollection<OrderItemEntity> OrderItems { get; set; }
        public ICollection<FeedbackEntity> Feedbacks { get; set; }
        public ICollection<ProductVersionEntity> ProductVersions { get; set; }
        public ICollection<UserRecommendationsEntity> UserRecommendations { get; set; }
    }
}
