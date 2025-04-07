using FoodFlowSystem.Entities.Invoice;

namespace FoodFlowSystem.Entities.Payment
{
    public class PaymentEntity : BaseEntity
    {
        public decimal Amount { get; set; }
        public string PaymentType { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public int InvoiceId { get; set; }
        public InvoiceEntity Invoice { get; set; }
    }
}
