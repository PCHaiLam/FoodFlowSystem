using FoodFlowSystem.DTOs.Requests.Order;
using FoodFlowSystem.Services.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodFlowSystem.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var order = await _orderService.CreateOrderAsync(request);
            return Ok(order);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest request)
        {
            var order = await _orderService.UpdateOrderAsync(request);
            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder([FromBody] int id)
        {
            var order = await _orderService.GetOrderById(id);
            return Ok(order);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetOrderByUserId([FromQuery] int id)
        {
            var orders = await _orderService.GetOrdersByUserId(id);
            return Ok(orders);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrder([FromBody] int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return Ok();
        }

        [HttpPost("by-date")]
        public async Task<IActionResult> GetOrderByDate([FromBody] DateTime date)
        {
            var orders = await _orderService.GetOrderByDate(date);
            return Ok(orders);
        }

        [HttpPost("by-range-date")]
        public async Task<IActionResult> GetOrderRangeDate([FromBody] DateTime startDate, DateTime endDate)
        {
            var orders = await _orderService.GetOrderRangeDate(startDate, endDate);
            return Ok(orders);
        }
    }
}
