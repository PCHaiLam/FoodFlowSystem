using FoodFlowSystem.DTOs.Requests.Table;
using FoodFlowSystem.Services.Table;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodFlowSystem.Controllers
{
    [Route("api/table")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTableByIdAsync([FromQuery] int id)
        {
            var table = await _tableService.GetTableByIdAsync(id);
            return Ok(table);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetTablesAsync()
        {
            var tables = await _tableService.GetTablesAsync();
            return Ok(tables);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTableAsync([FromBody] CreateTableRequest request)
        {
            var table = await _tableService.CreateTableAsync(request);
            return CreatedAtAction(nameof(GetTableByIdAsync), new { id = table.Id }, table);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTableAsync([FromQuery] int id)
        {
            await _tableService.DeleteTableAsync(id);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTableAsync([FromBody] UpdateTableRequest request)
        {
            var table = await _tableService.UpdateTableAsync(request);
            return Ok(table);
        }
    }
}
