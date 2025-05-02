using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities.EmailTemplate;
using Microsoft.EntityFrameworkCore;

namespace FoodFlowSystem.Repositories.EmailTemplates
{
    public class EmailTemplatesRepository : BaseRepository<EmailTemplatesEntity>, IEmailTemplatesRepository
    {
        public EmailTemplatesRepository(MssqlDbContext context) : base(context)
        {
        }

        public async Task<EmailTemplatesEntity> GetTemplateByNameAsync(string name)
        {
            return await _dbContext.EmailTemplates.FirstOrDefaultAsync(x => x.TemplateName == name);
        }
    }
}
