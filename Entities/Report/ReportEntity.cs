using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Entities.Report
{
    public class ReportEntity : BaseEntity
    {
        public string Title { get; set; }
        public string ReportType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ReportContent { get; set; }
        public int? UserID { get; set; }
        public UserEntity User { get; set; }
    }
}
