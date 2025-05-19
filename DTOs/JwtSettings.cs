namespace FoodFlowSystem.DTOs
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpiryInMinutes { get; set; } = 30;
        public int RefreshTokenExpiryInDays { get; set; } = 7;
    }
}
