using FoodFlowSystem.Entities.Feedback;
using FoodFlowSystem.Entities.Invoice;
using FoodFlowSystem.Entities.OrderItem;
using FoodFlowSystem.Entities.Payment;
using FoodFlowSystem.Entities.Reservation;
using FoodFlowSystem.Entities.Table;
using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Entities.Order
{
    public class OrderEntity : BaseEntity
    {
        public decimal? TotalAmount { get; set; }
        public string Status { get; set; }
        public int UserID { get; set; }
        public int? ReservationID { get; set; }
        public int? TableID { get; set; }
        public UserEntity User { get; set; }
        public TableEntity Table { get; set; }
        public ReservationEntity Reservation { get; set; }
        public ICollection<InvoiceEntity> Invoices { get; set; }
        public ICollection<OrderItemEntity> OrderItems { get; set; }
        public ICollection<PaymentEntity> Payments { get; set; }

    }
}
