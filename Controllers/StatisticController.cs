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

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var result = await _statisticService.GetDashboardSummaryAsync();
            return Ok(result);
        }

        [HttpGet("revenue/daily")]
        public async Task<IActionResult> GetDailyRevenue([FromQuery] DateTime? date = null)
        {
            var targetDate = date ?? DateTime.Now;
            var result = await _statisticService.GetRevenueByDateAsync(targetDate);
            return Ok(result);
        }

        [HttpGet("revenue/weekly")]
        public async Task<IActionResult> GetWeeklyRevenue([FromQuery] DateTime? startDate = null)
        {
            var targetDate = startDate ?? DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            var result = await _statisticService.GetRevenueByWeekAsync(targetDate);
            return Ok(result);
        }

        [HttpGet("revenue/monthly")]
        public async Task<IActionResult> GetMonthlyRevenue([FromQuery] int? month = null, [FromQuery] int? year = null)
        {
            var targetMonth = month ?? DateTime.Now.Month;
            var targetYear = year ?? DateTime.Now.Year;
            var result = await _statisticService.GetRevenueByMonthAsync(targetMonth, targetYear);
            return Ok(result);
        }

        [HttpGet("revenue/quarterly")]
        public async Task<IActionResult> GetQuarterlyRevenue([FromQuery] int? quarter = null, [FromQuery] int? year = null)
        {
            var targetQuarter = quarter ?? ((DateTime.Now.Month - 1) / 3) + 1;
            var targetYear = year ?? DateTime.Now.Year;
            var result = await _statisticService.GetRevenueByQuarterAsync(targetQuarter, targetYear);
            return Ok(result);
        }

        [HttpGet("revenue/yearly")]
        public async Task<IActionResult> GetYearlyRevenue([FromQuery] int? year = null)
        {
            var targetYear = year ?? DateTime.Now.Year;
            var result = await _statisticService.GetRevenueByYearAsync(targetYear);
            return Ok(result);
        }

        [HttpGet("products/daily")]
        public async Task<IActionResult> GetDailyProductStats([FromQuery] DateTime? date = null)
        {
            var targetDate = date ?? DateTime.Now;
            var result = await _statisticService.GetProductStatisticsByDateAsync(targetDate);
            return Ok(result);
        }

        [HttpGet("products/weekly")]
        public async Task<IActionResult> GetWeeklyProductStats([FromQuery] DateTime? startDate = null)
        {
            var targetDate = startDate ?? DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            var result = await _statisticService.GetProductStatisticsByWeekAsync(targetDate);
            return Ok(result);
        }

        [HttpGet("products/monthly")]
        public async Task<IActionResult> GetMonthlyProductStats([FromQuery] int? month = null, [FromQuery] int? year = null)
        {
            var targetMonth = month ?? DateTime.Now.Month;
            var targetYear = year ?? DateTime.Now.Year;
            var result = await _statisticService.GetProductStatisticsByMonthAsync(targetMonth, targetYear);
            return Ok(result);
        }
    }
} 