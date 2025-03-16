namespace FoodFlowSystem.DTOs.Responses
{
    public class OrderResponse
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public int ReservationID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<OrderItemResponse> ListOrderItems { get; set; }
    }
}
