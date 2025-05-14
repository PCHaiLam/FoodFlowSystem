using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodFlowSystem.Repositories.Statistic
{
    public interface IStatisticRepository
    {
        Task<(decimal TotalRevenue, int OrderCount)> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<(DateTime Date, decimal Revenue, int OrderCount)>> GetDailyRevenueInRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<(int ProductId, string ProductName, int QuantitySold, decimal Revenue, string ImageUrl)>> GetTopSellingProductsInRangeAsync(DateTime startDate, DateTime endDate, int limit = 10);
        Task<List<(int CategoryId, string CategoryName, int QuantitySold, decimal Revenue)>> GetCategorySalesInRangeAsync(DateTime startDate, DateTime endDate);
        Task<int> GetTotalCustomersAsync();
        Task<int> GetNewCustomersInRangeAsync(DateTime startDate, DateTime endDate);
        Task<(int Pending, int Completed)> GetOrderStatusCountsAsync();
        Task<List<(int OrderId, string CustomerName, DateTime OrderDate, decimal TotalAmount, string Status)>> GetRecentOrdersAsync(int limit = 5);
    }
} 