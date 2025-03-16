using FoodFlowSystem.Entities.Invoice;
using FoodFlowSystem.Entities.Order;

namespace FoodFlowSystem.Entities.Payment
{
    public class PaymentEntity : BaseEntity
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public int OrderID { get; set; }
        public OrderEntity Order { get; set; }
        public ICollection<InvoiceEntity> Invoices { get; set; }
    }
}
