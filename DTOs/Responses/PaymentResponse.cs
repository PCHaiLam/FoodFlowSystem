namespace FoodFlowSystem.DTOs.Responses
{
    public class PaymentResponse
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string PaymentType { get; set; }
        public string PaymentMethod { get; set; }
        public string Amount { get; set; }
        public bool IsDeposit { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
