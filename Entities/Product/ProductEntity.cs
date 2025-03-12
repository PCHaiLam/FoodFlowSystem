using FoodFlowSystem.Entities.Category;
using FoodFlowSystem.Entities.Feedback;
using FoodFlowSystem.Entities.OrderItem;

namespace FoodFlowSystem.Entities.Product
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }
        public int? CategoryID { get; set; }
        public CategoryEntity Category { get; set; }
        public ICollection<OrderItemEntity> OrderItems { get; set; }
        public ICollection<FeedbackEntity> Feedbacks { get; set; }

    }
}
