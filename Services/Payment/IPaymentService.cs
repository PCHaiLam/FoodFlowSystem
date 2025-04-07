using FoodFlowSystem.DTOs.Requests.Payment;
using FoodFlowSystem.DTOs.Responses;

namespace FoodFlowSystem.Services.Payment
{
    public interface IPaymentService
    {
        Task<PaymentResponse> CreatePaymentAsync(CreatePaymentRequest request);
        //Task<ICollection<PaymentResponse>> GetPaymentByInvoiceIdAsync(int id);
        Task<ICollection<PaymentResponse>> GetAllPaymentsAsync();
    }
}
