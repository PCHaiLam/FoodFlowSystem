namespace FoodFlowSystem.DTOs.Responses
{
    public class OrderItemResponse
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
