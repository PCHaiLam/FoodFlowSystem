using FoodFlowSystem.Entities.Order;
using FoodFlowSystem.Entities.Payment;

namespace FoodFlowSystem.Entities.Invoice
{
    public class InvoiceEntity : BaseEntity
    {
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public int OrderID { get; set; }
        public OrderEntity Order { get; set; }
        public ICollection<PaymentEntity> Payments { get; set; }
    }
}
