using FoodFlowSystem.Contexts;
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
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _productService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetAllActiveAsync(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string filter = null,
            [FromQuery] string search = null,
            [FromQuery] string category = null,
            [FromQuery] int? minPrice = null,
            [FromQuery] int? maxPrice = null,
            [FromQuery] string rating = null,
            [FromQuery] string sort = null)
        {
            var count = await _productService.CountAllActive();

            if (filter == "all")
            {
                this.HttpContext.SetPaginationInfo(count, page, size);
            }

            var result = await _productService.GetAllActiveAsync(page, size, filter, search, category, minPrice, maxPrice, rating, sort);

            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] int id)
        {
            var result = await _productService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "1,3")]
        public async Task<IActionResult> AddAsync([FromBody] CreateProductRequest request)
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
