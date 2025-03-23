using FoodFlowSystem.Services.Invoice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodFlowSystem.Controllers
{
    [Route("api/invoice")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoiceById([FromQuery] int id)
        {
            var invoices = await _invoiceService.GetInvoiceByIdAsync(id);
            return Ok(invoices);
        }
    }
}
