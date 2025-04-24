namespace FoodFlowSystem.DTOs.Requests.Payment.PaymentConfigs
{
    public class VNPayConfig
    {
        public string Version { get; set; }
        public string TmnCode { get; set; }
        public string HashSecret { get; set; }
        public string PaymentUrl { get; set; }
        public string ReturnUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
