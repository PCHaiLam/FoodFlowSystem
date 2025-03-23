using AutoMapper;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Middlewares.Exceptions;
using FoodFlowSystem.Repositories.Invoice;

namespace FoodFlowSystem.Services.Invoice
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ILogger<InvoiceService> _logger;
        private readonly IMapper _mapper;

        public InvoiceService(IInvoiceRepository invoiceRepository, ILogger<InvoiceService> logger, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<InvoiceResponse> GetInvoiceByIdAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
            {
                _logger.LogError($"Invoice not exist.");
                throw new ApiException("Invoice not exist", 404);
            }

            _logger.LogInformation($"Get invoice completed.");

            var result = _mapper.Map<InvoiceResponse>(invoice);
            return result;
        }
    }
}
