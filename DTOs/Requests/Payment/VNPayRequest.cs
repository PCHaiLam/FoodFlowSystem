namespace FoodFlowSystem.DTOs.Requests.Payment
{
    public class VNPayRequest
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string OrderInfo { get; set; }
    }
}
