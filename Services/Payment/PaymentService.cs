using AutoMapper;
using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Payment;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Entities.Payment;
using FoodFlowSystem.Middlewares.Exceptions;
using FoodFlowSystem.Repositories.Payment;
using System.CodeDom;

namespace FoodFlowSystem.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;
        private readonly IValidator<CreatePaymentRequest> _createValidator;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IMapper mapper,
            ILogger<PaymentService> logger,
            IValidator<CreatePaymentRequest> createValidator
            )
        {
            _paymentRepository = paymentRepository;
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

            var payment = _mapper.Map<PaymentEntity>(request);
            var paymentDto = await _paymentRepository.AddAsync(payment);

            var result = _mapper.Map<PaymentResponse>(paymentDto);

            return result;
        }

        public async Task<ICollection<PaymentResponse>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            var result = _mapper.Map<ICollection<PaymentResponse>>(payments);

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
