using FoodFlowSystem.Entities.EmailTemplate;

namespace FoodFlowSystem.Repositories.EmailTemplates
{
    public interface IEmailTemplatesRepository : IBaseRepository<EmailTemplatesEntity>
    {
        Task<EmailTemplatesEntity> GetTemplateByNameAsync(string name);
    }
}
