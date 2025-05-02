using FoodFlowSystem.DTOs.Requests.Payment;
using FoodFlowSystem.DTOs.Responses.Payments;
using FoodFlowSystem.Services.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodFlowSystem.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IVNPayService _vnPayService;

        public PaymentController(IPaymentService paymentService, IVNPayService vNPayService)
        {
            _paymentService = paymentService;
            _vnPayService = vNPayService;
        }

        [HttpPatch]
        public async Task<IActionResult> PaymentConfirmationAsync([FromBody] PaymentConfirmationRequest request)
        {
            var payments = await _paymentService.PaymentConfirmationAsync(request);
            return Ok(payments);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            var payment = await _paymentService.CreatePaymentAsync(request);
            return Ok(payment);
        }

        [HttpGet("vnpay-callback")]
        public async Task<IActionResult> VnPayCallback()
        {
            var response = _vnPayService.ProcessPaymentCallback(Request.Query);
            var result = await _paymentService.ProcessVNPayCallbackAsync(response);

            var returnUrl = $"http://localhost:5173/payment-information/{result.OrderId}";
            return Redirect(returnUrl);
        }
    }
}
