namespace FoodFlowSystem.DTOs.Requests.Payment.PaymentConfigs
{
    public class MoMoConfig
    {
        public string PartnerCode { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string PaymentUrl { get; set; }
        public string ReturnUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
