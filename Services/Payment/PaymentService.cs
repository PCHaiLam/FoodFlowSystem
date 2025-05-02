using AutoMapper;
using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Payment;
using FoodFlowSystem.DTOs.Responses.Payments;
using FoodFlowSystem.Entities.Payment;
using FoodFlowSystem.DTOs;
using FoodFlowSystem.Repositories.Invoice;
using FoodFlowSystem.Repositories.Order;
using FoodFlowSystem.Repositories.Payment;
using FoodFlowSystem.Services.Order;
using System.CodeDom;
using FoodFlowSystem.Repositories.EmailTemplates;
using FoodFlowSystem.Services.SendMail;
using FoodFlowSystem.Entities.Order;
using System.Text;

namespace FoodFlowSystem.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailTemplatesRepository _emailTemplatesRepository;
        private readonly ISendMailService _sendMailService;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;
        private readonly IValidator<CreatePaymentRequest> _createValidator;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IInvoiceRepository invoiceRepository,
            IOrderRepository orderRepository,
            IEmailTemplatesRepository emailTemplatesRepository,
            ISendMailService sendMailService,
            IMapper mapper,
            ILogger<PaymentService> logger,
            IValidator<CreatePaymentRequest> createValidator
            )
        {
            _paymentRepository = paymentRepository;
            _invoiceRepository = invoiceRepository;
            _orderRepository = orderRepository;
            _emailTemplatesRepository = emailTemplatesRepository;
            _sendMailService = sendMailService;
            _mapper = mapper;
            _logger = logger;
            _createValidator = createValidator;
        }

        public async Task<PaymentResponse> CreatePaymentAsync(CreatePaymentRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
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

        public async Task<bool> PaymentConfirmationAsync(PaymentConfirmationRequest request)
        {
            var order = await _orderRepository.GetOrderDetailByIdAsync(request.OrderId);
            if (order == null)
            {
                throw new ApiException("Không có đơn hàng", 404);
            }
            if (order.Status != "Pending")
            {
                throw new ApiException("Đơn hàng đã được thanh toán", 400);
            }

            order.Status = "Completed";
            await _orderRepository.UpdateAsync(order);
            await SendEmailAsync(order);

            return true;
        }

        public async Task<PaymentResponse> ProcessVNPayCallbackAsync(VNPayResponse response)
        {
            if (response.ResponseCode != "00")
            {
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

        private async Task SendEmailAsync(OrderEntity order)
        {
            var emailTemplate = await _emailTemplatesRepository.GetTemplateByNameAsync("OrderConfirm");
            if (emailTemplate == null)
            {
                throw new ApiException("Email template not found", 404);
            }

            var emailSubject = emailTemplate.Subject
                .Replace("{orderId}", order.ID.ToString());

            var emailBody = emailTemplate.Body
                .Replace("{orderId}", order.ID.ToString())
                .Replace("{fullName}", order.User.LastName + " " + order.User.FirstName)
                .Replace("{orderDate}", order.CreatedAt.ToString("dd/MM/yyyy HH:mm"))
                .Replace("{totalAmount}", string.Format("{0:#,##0} VNĐ", order.TotalAmount))
                .Replace("{web}", "http://localhost:5173/")
                .Replace("{orderTrackingUrl}", $"http://localhost:5173/order-tracking/{order.ID}");

            var orderItemsHtml = new StringBuilder();
            foreach (var item in order.OrderItems)
            {
                orderItemsHtml.Append($@"
                <tr>
                    <td style=""border: 1px solid #ddd; padding: 8px;"">{item.Product.Name}</td>
                    <td style=""border: 1px solid #ddd; padding: 8px; text-align: center;"">{item.Quantity}</td>
                    <td style=""border: 1px solid #ddd; padding: 8px; text-align: right;"">{string.Format("{0:#,##0} VNĐ", item.Price)}</td>
                    <td style=""border: 1px solid #ddd; padding: 8px; text-align: right;"">{string.Format("{0:#,##0} VNĐ", item.Price * item.Quantity)}</td>
                </tr>");
            }
            emailBody = emailBody.Replace("{orderItems}", orderItemsHtml.ToString());

            await _sendMailService.SendMailAsync(order.User.Email, emailSubject, emailBody);
        }
    }
}
