namespace FoodFlowSystem.DTOs.Responses
{
    public class OrderItemResponse
    {
        public int OrderItemId { get; set; }
        public int OrderID { get; set; }
        public int ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
