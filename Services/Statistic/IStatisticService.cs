using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodFlowSystem.DTOs.Responses.Statistic;

namespace FoodFlowSystem.Services.Statistic
{
    public interface IStatisticService
    {
        Task<RevenueStatisticResponse> GetRevenueByDateAsync(DateTime date);
        Task<RevenueStatisticResponse> GetRevenueByWeekAsync(DateTime startDate);
        Task<RevenueStatisticResponse> GetRevenueByMonthAsync(int month, int year);
        Task<RevenueStatisticResponse> GetRevenueByQuarterAsync(int quarter, int year);
        Task<RevenueStatisticResponse> GetRevenueByYearAsync(int year);
        Task<ProductStatisticResponse> GetProductStatisticsByDateAsync(DateTime date);
        Task<ProductStatisticResponse> GetProductStatisticsByWeekAsync(DateTime startDate);
        Task<ProductStatisticResponse> GetProductStatisticsByMonthAsync(int month, int year);
        Task<DashboardSummaryResponse> GetDashboardSummaryAsync();
    }
} 