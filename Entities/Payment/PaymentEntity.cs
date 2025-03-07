using FoodFlowSystem.Entities.Invoice;
using FoodFlowSystem.Entities.Order;
using FoodFlowSystem.Entities.Reservation;

namespace FoodFlowSystem.Entities.Payment
{
    public class PaymentEntity : BaseEntity
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public bool IsDeposit { get; set; }
        public int OrderID { get; set; }
        public int? ReservationID { get; set; }
        public OrderEntity Order { get; set; }
        public ReservationEntity Reservation { get; set; }
        public ICollection<InvoiceEntity> Invoices { get; set; }
    }
}
