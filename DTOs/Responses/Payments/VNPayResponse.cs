namespace FoodFlowSystem.DTOs.Responses.Payments
{
    public class VNPayResponse
    {
        public int OrderId { get; set; }
        public string TransactionId { get; set; }
        public string ResponseCode { get; set; }
        public decimal Amount { get; set; }
        public string Message { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool Success => ResponseCode == "00";
    }
}
