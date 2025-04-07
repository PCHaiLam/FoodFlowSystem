using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities.Invoice;

namespace FoodFlowSystem.Repositories.Invoice
{
    public class InvoiceRepository : BaseRepository<InvoiceEntity>, IInvoiceRepository
    {
        public InvoiceRepository(MssqlDbContext context) : base(context)
        {
        }
    }
}
