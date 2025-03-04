using FoodFlowSystem.Entities.Order;
using FoodFlowSystem.Entities.Payment;
using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Entities.Invoice
{
    public class InvoiceEntity : BaseEntity
    {
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public int GeneratedBy { get; set; }
        public int PaymentID { get; set; }
        public int OrderID { get; set; }
        public PaymentEntity Payment { get; set; }
        public OrderEntity Order { get; set; }

    }
}
