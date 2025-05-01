using FoodFlowSystem.Entities.Product;
using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Entities.UserRecommendations
{
    public class UserRecommendationsEntity : BaseEntity
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public float Score { get; set; }

        public UserEntity User { get; set; }
        public ProductEntity Product { get; set; }
    }
}
