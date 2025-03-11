namespace FoodFlowSystem.DTOs.Requests.Auth
{
    public class GoogleUserProfile
    {
        public string Email { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Picture { get; set; } = default!;
    }
}
