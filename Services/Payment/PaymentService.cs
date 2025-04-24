using AutoMapper;
using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Payment;
using FoodFlowSystem.DTOs.Responses.Payments;
using FoodFlowSystem.Entities.Payment;
using FoodFlowSystem.Middlewares.Exceptions;
using FoodFlowSystem.Repositories.Invoice;
using FoodFlowSystem.Repositories.Payment;
using System.CodeDom;

namespace FoodFlowSystem.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;
        private readonly IValidator<CreatePaymentRequest> _createValidator;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IInvoiceRepository invoiceRepository,
            IMapper mapper,
            ILogger<PaymentService> logger,
            IValidator<CreatePaymentRequest> createValidator
            )
        {
            _paymentRepository = paymentRepository;
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
            _logger = logger;
            _createValidator = createValidator;
        }

        public async Task<PaymentResponse> CreatePaymentAsync(CreatePaymentRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogError("Validation failed");
                var errors = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                });
                throw new ApiException("Invalid input", 400, errors);
            }

            var mapNewPayment = _mapper.Map<PaymentEntity>(request);
            mapNewPayment.Status = "Pending";
            var newPayment = await _paymentRepository.AddAsync(mapNewPayment);

            var result = _mapper.Map<PaymentResponse>(newPayment);

            return result;
        }

        public async Task<ICollection<PaymentResponse>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            var result = _mapper.Map<ICollection<PaymentResponse>>(payments);

            return result;
        }

        public async Task<PaymentResponse> ProcessVNPayCallbackAsync(VNPayResponse response)
        {
            if (response.ResponseCode != "00")
            {
                _logger.LogError($"Payment failed: {response.Message}");
                throw new ApiException("Giao dịch thất bại.", 400);
            }

            var payment = await _paymentRepository.GetPendingDepositPaymentsByOrderId(response.OrderId);
            if (payment == null)
            {
                throw new ApiException("Lỗi khi thanh toán", 400);
            }

            payment.Status = "Completed";
            //payment.TransactionId = response.TransactionId;
            payment.Amount = response.Amount;

            await _paymentRepository.UpdateAsync(payment);

            var result = _mapper.Map<PaymentResponse>(payment);
            result.OrderId = response.OrderId;
            result.IsDeposit = true;
            result.Message = response.Message;

            return result;
        }

        //public async Task<ICollection<PaymentResponse>> GetPaymentByInvoiceIdAsync(int id)
        //{
        //    var payments = await _paymentRepository.GetByIdAsync(id);
        //    var result = _mapper.Map<ICollection<PaymentResponse>>(payments);

        //    return result;
        //}
    }
}
