using System;
using System.Collections.Generic;

namespace FoodFlowSystem.DTOs.Responses.Statistic
{
    public class DashboardSummaryResponse
    {
        public decimal TodayRevenue { get; set; }
        public decimal WeekRevenue { get; set; }
        public decimal MonthRevenue { get; set; }
        public decimal YearRevenue { get; set; }
        
        public int TodayOrders { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        
        public int TotalCustomers { get; set; }
        public int NewCustomers { get; set; }
        
        public List<ProductSaleDetail> TopSellingProducts { get; set; } = new List<ProductSaleDetail>();
        public List<RecentOrder> RecentOrders { get; set; } = new List<RecentOrder>();
    }

    public class RecentOrder
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
} 