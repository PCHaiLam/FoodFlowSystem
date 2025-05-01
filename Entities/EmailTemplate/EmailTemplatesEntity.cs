namespace FoodFlowSystem.Entities.EmailTemplate
{
    public class EmailTemplatesEntity : BaseEntity
    {
        public string TemplateName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsActive { get; set; }
    }
}
