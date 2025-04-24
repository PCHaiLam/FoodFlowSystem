namespace FoodFlowSystem.Entities
{
    public class JwtSettingClass
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpiryInMinutes { get; set; } = 30;
    }
}
