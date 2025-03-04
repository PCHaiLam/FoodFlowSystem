using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Entities.AuditLog
{
    public class AuditLogEntity : BaseEntity
    {
        public string Action { get; set; }
        public string TableName { get; set; }
        public string RecordID { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public int? UserID { get; set; }
        public UserEntity User { get; set; }
    }
}
