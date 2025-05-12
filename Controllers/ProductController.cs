using FoodFlowSystem.Extensions;
using FoodFlowSystem.DTOs.Requests.Product;
using FoodFlowSystem.Services.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodFlowSystem.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Authorize(Roles = "1,3")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _productService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActiveAsync([FromQuery] string filter = null, [FromQuery] string quickFilter = null, [FromQuery] string rating = null)
        {
            var result = await _productService.GetAllActiveAsync(filter);

            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] int id)
        {
            var result = await _productService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> AddAsync([FromForm] CreateProductRequest request)
        {
            var result = await _productService.AddAsync(request);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "1,3")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateProductRequest request)
        {
            var result = await _productService.UpdateAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _productService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("name")]
        public async Task<IActionResult> GetByNameAsync([FromQuery] string name)
        {
            var result = await _productService.GetByNameAsync(name);
            return Ok(result);
        }

        [HttpGet("price")]
        public async Task<IActionResult> GetByPriceAsync([FromQuery] decimal price)
        {
            var result = await _productService.GetByPriceAsync(price);
            return Ok(result);
        }
    }
}
