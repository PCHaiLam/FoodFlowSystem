using System;
using System.Threading.Tasks;
using FoodFlowSystem.Services.Statistic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodFlowSystem.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    [Authorize(Roles = "1")] // Assuming role 1 is for admin
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStatisticByRangeDate([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _statisticService.GetStatisticByArangeDateAsync(startDate, endDate);
            return Ok(result);
        }
    }
}