using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Entities.Token
{
    public class TokenEntity : BaseEntity
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserID { get; set; }
        public UserEntity User { get; set; }
    }
}
