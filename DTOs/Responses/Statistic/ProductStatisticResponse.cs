namespace FoodFlowSystem.DTOs.Responses.Statistic
{
    public class ProductStatisticResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
