using System;

namespace FoodFlowSystem.Entities.Token
{
    public class TokenEntity : BaseEntity
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public int UserId { get; set; }
        
        // Navigation property
        public virtual Entities.User.UserEntity User { get; set; }
    }
} 