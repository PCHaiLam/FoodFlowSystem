using Microsoft.EntityFrameworkCore;
using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities.Invoice;

namespace FoodFlowSystem.Repositories.Invoice
{
    public class InvoiceRepository : BaseRepository<InvoiceEntity>, IInvoiceRepository
    {
        public InvoiceRepository(MssqlDbContext context) : base(context)
        {
        }

        public async Task<ICollection<InvoiceEntity>> GetByArangeDate(DateTime startDate, DateTime endDate)
        {
            var result = await _dbContext.Invoices
                .Include(x => x.Order)
                .Where(x => x.Order.UpdatedAt >= startDate && x.Order.UpdatedAt <= endDate)
                .ToListAsync();

            return result;
        }
    }
}
