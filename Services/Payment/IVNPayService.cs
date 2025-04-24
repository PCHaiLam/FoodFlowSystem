using FoodFlowSystem.DTOs.Requests.Payment;
using FoodFlowSystem.DTOs.Responses.Payments;

namespace FoodFlowSystem.Services.Payment
{
    public interface IVNPayService
    {
        string CreatePaymentUrl(VNPayRequest request, string ipAddress);
        VNPayResponse ProcessPaymentCallback(IQueryCollection query);
    }
}
