namespace FoodFlowSystem.DTOs.Requests.Table
{
    public class CreateTableRequest
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public int Capacity { get; set; }
    }
}
