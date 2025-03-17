using FoodFlowSystem.Entities.Order;
using FoodFlowSystem.Entities.Reservation;

namespace FoodFlowSystem.Entities.Table
{
    public class TableEntity : BaseEntity
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
        public ICollection<ReservationEntity> Reservations { get; set; }
        public ICollection<OrderEntity> Orders { get; set; }
    }
}
