using FoodFlowSystem.DTOs.Requests.Payment;
using FoodFlowSystem.DTOs.Responses.Payments;

namespace FoodFlowSystem.Services.Payment
{
    public interface IPaymentService
    {
        Task<PaymentResponse> CreatePaymentAsync(CreatePaymentRequest request);
        Task<PaymentResponse> ProcessVNPayCallbackAsync(VNPayResponse response);
    }
}
