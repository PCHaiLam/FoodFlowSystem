namespace FoodFlowSystem.DTOs.Requests.Table
{
    public class UpdateTableRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
    }
}
