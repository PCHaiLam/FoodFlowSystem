using FoodFlowSystem.Entities.Order;
using FoodFlowSystem.Entities.Table;
using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Entities.Reservation
{
    public class ReservationEntity : BaseEntity
    {
        public int NumberOfGuests { get; set; }
        public string Status { get; set; }
        public int? UserID { get; set; }
        public int? TableID { get; set; }
        public UserEntity User { get; set; }
        public TableEntity Table { get; set; }
        public ICollection<OrderEntity> Orders { get; set; }
    }
}
