using FoodFlowSystem.Entities.User;
using System;

namespace FoodFlowSystem.Entities.Token
{
    public class TokenEntity : BaseEntity
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int UserID { get; set; }
        public UserEntity User { get; set; }
    }
} 