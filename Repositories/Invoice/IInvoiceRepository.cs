using FoodFlowSystem.Entities.Invoice;

namespace FoodFlowSystem.Repositories.Invoice
{
    public interface IInvoiceRepository : IBaseRepository<InvoiceEntity>
    {
        Task<ICollection<InvoiceEntity>> GetByArangeDate(DateTime startDate, DateTime endDate);
    }
}
