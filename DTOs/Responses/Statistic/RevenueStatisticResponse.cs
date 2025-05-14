using System;
using System.Collections.Generic;

namespace FoodFlowSystem.DTOs.Responses.Statistic
{
    public class RevenueStatisticResponse
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public List<DailyRevenueData> RevenueDetails { get; set; } = new List<DailyRevenueData>();
        public string Period { get; set; }
    }

    public class DailyRevenueData
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }
} 