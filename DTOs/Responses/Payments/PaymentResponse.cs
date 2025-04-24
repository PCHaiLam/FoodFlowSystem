namespace FoodFlowSystem.DTOs.Responses.Payments
{
    public class PaymentResponse
    {
        public int OrderId { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public string PaymentType { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public bool IsDeposit { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
