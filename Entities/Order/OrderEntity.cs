using FoodFlowSystem.Entities.Invoice;
using FoodFlowSystem.Entities.OrderItem;
using FoodFlowSystem.Entities.Table;
using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Entities.Order
{
    public class OrderEntity : BaseEntity
    {
        public string OrderType { get; set; }
        public int? NumOfGuests { get; set; }
        public DateTime? ReservationDate { get; set; }
        public TimeOnly? ReservationTime { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public int UserID { get; set; }
        public int? TableID { get; set; }
        public UserEntity User { get; set; }
        public TableEntity Table { get; set; }
        public ICollection<OrderItemEntity> OrderItems { get; set; }
        public ICollection<InvoiceEntity> Invoices { get; set; }
    }
}
