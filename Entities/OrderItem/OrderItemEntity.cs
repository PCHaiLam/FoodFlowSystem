using FoodFlowSystem.Entities.Order;
using FoodFlowSystem.Entities.Product;

namespace FoodFlowSystem.Entities.OrderItem
{
    public class OrderItemEntity : BaseEntity
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public OrderEntity Order { get; set; }
        public ProductEntity Product { get; set; }
    }
}
