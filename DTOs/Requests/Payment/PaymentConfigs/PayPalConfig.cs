namespace FoodFlowSystem.DTOs.Requests.Payment.PaymentConfigs
{
    public class PayPalConfig
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Mode { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
