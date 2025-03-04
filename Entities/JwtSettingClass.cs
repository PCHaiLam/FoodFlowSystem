namespace FoodFlowSystem.Entities
{
    public class JwtSettingClass
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryInMinutes { get; set; }
    }
}
