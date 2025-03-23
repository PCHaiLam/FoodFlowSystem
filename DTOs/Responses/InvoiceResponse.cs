using FoodFlowSystem.Entities.Payment;

namespace FoodFlowSystem.DTOs.Responses
{
    public class InvoiceResponse
    {
        public int InvoiceId { get; set; }
        public decimal TotalAmount { get; set; }
        public int Discount { get; set; }
        public OrderResponse Order { get; set; }
        public ICollection<PaymentResponse> Payments { get; set; }
    }
}
