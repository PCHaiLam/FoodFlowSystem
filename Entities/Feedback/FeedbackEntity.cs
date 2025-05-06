using FoodFlowSystem.Entities.Product;
using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Entities.Feedback
{
    public class FeedbackEntity : BaseEntity
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public int? OrderID { get; set; }
        public ProductEntity Product { get; set; }
        public UserEntity User { get; set; }
    }
}
