namespace FoodFlowSystem.DTOs.Requests.OrderItem
{
    public class CreateOrderItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
    }
}
