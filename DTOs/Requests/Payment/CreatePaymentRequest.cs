namespace FoodFlowSystem.DTOs.Requests.Payment
{
    public class CreatePaymentRequest
    {
        public string PaymentType { get; set; }
        public string PaymentMethod { get; set; }
    }
}
