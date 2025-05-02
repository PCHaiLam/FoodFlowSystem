using FoodFlowSystem.DTOs.Requests.Payment;
using FoodFlowSystem.DTOs.Responses.Payments;

namespace FoodFlowSystem.Services.Payment
{
    public interface IPaymentService
    {
        Task<bool> PaymentConfirmationAsync(PaymentConfirmationRequest request);
        Task<PaymentResponse> CreatePaymentAsync(CreatePaymentRequest request);
        Task<PaymentResponse> ProcessVNPayCallbackAsync(VNPayResponse response);
    }
}
