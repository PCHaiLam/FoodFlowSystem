using FoodFlowSystem.DTOs.Responses.Recommendations;

namespace FoodFlowSystem.DTOs.Responses.Statistic
{
    public class DailyStatisticResponse
    {
        public decimal DailyRevenue { get; set; }
        public ICollection<ProductStatisticResponse> ProductStatistics { get; set; }
        public ICollection<CustomerStatisticResponse> CustomerStatistics { get; set; }
        public ICollection<InvoiceResponse> InvoicesStatistic { get; set; }
    }
} 