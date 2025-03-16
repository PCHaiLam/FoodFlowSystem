namespace FoodFlowSystem.DTOs.Requests.OrderItem
{
    public class CreateOrderItemRequest
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
    }
}
