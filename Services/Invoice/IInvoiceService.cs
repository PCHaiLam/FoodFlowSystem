using FoodFlowSystem.DTOs.Responses;

namespace FoodFlowSystem.Services.Invoice
{
    public interface IInvoiceService
    {
        Task<InvoiceResponse> GetInvoiceByIdAsync(int id);

    }
}
