namespace FoodFlowSystem.Entities.SearchLogs
{
    public class SearchLogsEntity : BaseEntity
    {
        public string Keyword { get; set; }
        public int? UserId { get; set; }
    }
}
