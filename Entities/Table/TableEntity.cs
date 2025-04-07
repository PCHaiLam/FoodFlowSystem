using FoodFlowSystem.Entities.Order;

namespace FoodFlowSystem.Entities.Table
{
    public class TableEntity : BaseEntity
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
        public ICollection<OrderEntity> Orders { get; set; }
    }
}
