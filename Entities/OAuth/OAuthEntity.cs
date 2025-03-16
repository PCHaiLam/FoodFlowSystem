using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Entities.OAuth
{
    public class OAuthEntity : BaseEntity
    {
        public int UserId { get; set; }
        public string Provider { get; set; }
        public string ProviderUserId { get; set; }
        public string Email { get; set; }
        public DateTime LastLoginAt { get; set; }
        public UserEntity User { get; set; }
    }
}
