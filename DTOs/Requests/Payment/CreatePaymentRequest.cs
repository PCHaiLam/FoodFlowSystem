namespace FoodFlowSystem.DTOs.Requests.Payment
{
    public class CreatePaymentRequest
    {
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentType { get; set; }
        public string PaymentMethod { get; set; }
    }
}
