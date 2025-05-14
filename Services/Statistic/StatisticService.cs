using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodFlowSystem.DTOs.Responses.Statistic;
using FoodFlowSystem.Repositories.Statistic;

namespace FoodFlowSystem.Services.Statistic
{
    public class StatisticService : IStatisticService
    {
        private readonly IStatisticRepository _statisticRepository;

        public StatisticService(IStatisticRepository statisticRepository)
        {
            _statisticRepository = statisticRepository;
        }

        public async Task<RevenueStatisticResponse> GetRevenueByDateAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1).AddSeconds(-1);

            var (totalRevenue, orderCount) = await _statisticRepository.GetRevenueByDateRangeAsync(startDate, endDate);
            var dailyData = await _statisticRepository.GetDailyRevenueInRangeAsync(startDate, endDate);

            var response = new RevenueStatisticResponse
            {
                TotalRevenue = totalRevenue,
                TotalOrders = orderCount,
                AverageOrderValue = orderCount > 0 ? totalRevenue / orderCount : 0,
                Period = $"{startDate:yyyy-MM-dd}",
                RevenueDetails = dailyData.Select(d => new DailyRevenueData
                {
                    Date = d.Date,
                    Revenue = d.Revenue,
                    OrderCount = d.OrderCount
                }).ToList()
            };

            return response;
        }

        public async Task<RevenueStatisticResponse> GetRevenueByWeekAsync(DateTime startDate)
        {
            startDate = startDate.Date;
            var endDate = startDate.AddDays(7).AddSeconds(-1);

            var (totalRevenue, orderCount) = await _statisticRepository.GetRevenueByDateRangeAsync(startDate, endDate);
            var dailyData = await _statisticRepository.GetDailyRevenueInRangeAsync(startDate, endDate);

            var response = new RevenueStatisticResponse
            {
                TotalRevenue = totalRevenue,
                TotalOrders = orderCount,
                AverageOrderValue = orderCount > 0 ? totalRevenue / orderCount : 0,
                Period = $"{startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}",
                RevenueDetails = dailyData.Select(d => new DailyRevenueData
                {
                    Date = d.Date,
                    Revenue = d.Revenue,
                    OrderCount = d.OrderCount
                }).ToList()
            };

            return response;
        }

        public async Task<RevenueStatisticResponse> GetRevenueByMonthAsync(int month, int year)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);

            var (totalRevenue, orderCount) = await _statisticRepository.GetRevenueByDateRangeAsync(startDate, endDate);
            var dailyData = await _statisticRepository.GetDailyRevenueInRangeAsync(startDate, endDate);

            var response = new RevenueStatisticResponse
            {
                TotalRevenue = totalRevenue,
                TotalOrders = orderCount,
                AverageOrderValue = orderCount > 0 ? totalRevenue / orderCount : 0,
                Period = $"{startDate:MMMM yyyy}",
                RevenueDetails = dailyData.Select(d => new DailyRevenueData
                {
                    Date = d.Date,
                    Revenue = d.Revenue,
                    OrderCount = d.OrderCount
                }).ToList()
            };

            return response;
        }

        public async Task<RevenueStatisticResponse> GetRevenueByQuarterAsync(int quarter, int year)
        {
            if (quarter < 1 || quarter > 4)
                throw new ArgumentException("Quarter must be between 1 and 4");

            int startMonth = (quarter - 1) * 3 + 1;
            var startDate = new DateTime(year, startMonth, 1);
            var endDate = startDate.AddMonths(3).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);

            var (totalRevenue, orderCount) = await _statisticRepository.GetRevenueByDateRangeAsync(startDate, endDate);
            var dailyData = await _statisticRepository.GetDailyRevenueInRangeAsync(startDate, endDate);

            var response = new RevenueStatisticResponse
            {
                TotalRevenue = totalRevenue,
                TotalOrders = orderCount,
                AverageOrderValue = orderCount > 0 ? totalRevenue / orderCount : 0,
                Period = $"Q{quarter} {year}",
                RevenueDetails = dailyData.Select(d => new DailyRevenueData
                {
                    Date = d.Date,
                    Revenue = d.Revenue,
                    OrderCount = d.OrderCount
                }).ToList()
            };

            return response;
        }

        public async Task<RevenueStatisticResponse> GetRevenueByYearAsync(int year)
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = new DateTime(year, 12, 31, 23, 59, 59);

            var (totalRevenue, orderCount) = await _statisticRepository.GetRevenueByDateRangeAsync(startDate, endDate);
            var dailyData = await _statisticRepository.GetDailyRevenueInRangeAsync(startDate, endDate);

            var response = new RevenueStatisticResponse
            {
                TotalRevenue = totalRevenue,
                TotalOrders = orderCount,
                AverageOrderValue = orderCount > 0 ? totalRevenue / orderCount : 0,
                Period = $"{year}",
                RevenueDetails = dailyData.Select(d => new DailyRevenueData
                {
                    Date = d.Date,
                    Revenue = d.Revenue,
                    OrderCount = d.OrderCount
                }).ToList()
            };

            return response;
        }

        public async Task<ProductStatisticResponse> GetProductStatisticsByDateAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1).AddSeconds(-1);

            var topProducts = await _statisticRepository.GetTopSellingProductsInRangeAsync(startDate, endDate);
            var categoryStats = await _statisticRepository.GetCategorySalesInRangeAsync(startDate, endDate);

            var response = new ProductStatisticResponse
            {
                TotalProductsSold = topProducts.Sum(p => p.QuantitySold),
                Period = $"{startDate:yyyy-MM-dd}",
                TopSellingProducts = topProducts.Select(p => new ProductSaleDetail
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    QuantitySold = p.QuantitySold,
                    Revenue = p.Revenue,
                    ImageUrl = p.ImageUrl
                }).ToList(),
                SalesByCategory = categoryStats.Select(c => new CategorySaleDetail
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    QuantitySold = c.QuantitySold,
                    Revenue = c.Revenue
                }).ToList()
            };

            return response;
        }

        public async Task<ProductStatisticResponse> GetProductStatisticsByWeekAsync(DateTime startDate)
        {
            startDate = startDate.Date;
            var endDate = startDate.AddDays(7).AddSeconds(-1);

            var topProducts = await _statisticRepository.GetTopSellingProductsInRangeAsync(startDate, endDate);
            var categoryStats = await _statisticRepository.GetCategorySalesInRangeAsync(startDate, endDate);

            var response = new ProductStatisticResponse
            {
                TotalProductsSold = topProducts.Sum(p => p.QuantitySold),
                Period = $"{startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}",
                TopSellingProducts = topProducts.Select(p => new ProductSaleDetail
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    QuantitySold = p.QuantitySold,
                    Revenue = p.Revenue,
                    ImageUrl = p.ImageUrl
                }).ToList(),
                SalesByCategory = categoryStats.Select(c => new CategorySaleDetail
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    QuantitySold = c.QuantitySold,
                    Revenue = c.Revenue
                }).ToList()
            };

            return response;
        }

        public async Task<ProductStatisticResponse> GetProductStatisticsByMonthAsync(int month, int year)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);

            var topProducts = await _statisticRepository.GetTopSellingProductsInRangeAsync(startDate, endDate);
            var categoryStats = await _statisticRepository.GetCategorySalesInRangeAsync(startDate, endDate);

            var response = new ProductStatisticResponse
            {
                TotalProductsSold = topProducts.Sum(p => p.QuantitySold),
                Period = $"{startDate:MMMM yyyy}",
                TopSellingProducts = topProducts.Select(p => new ProductSaleDetail
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    QuantitySold = p.QuantitySold,
                    Revenue = p.Revenue,
                    ImageUrl = p.ImageUrl
                }).ToList(),
                SalesByCategory = categoryStats.Select(c => new CategorySaleDetail
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    QuantitySold = c.QuantitySold,
                    Revenue = c.Revenue
                }).ToList()
            };

            return response;
        }

        public async Task<DashboardSummaryResponse> GetDashboardSummaryAsync()
        {
            // Today's date
            var today = DateTime.Now.Date;
            var tomorrow = today.AddDays(1);
            
            // Week range
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7).AddSeconds(-1);
            
            // Month range
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
            
            // Year range
            var startOfYear = new DateTime(today.Year, 1, 1);
            var endOfYear = new DateTime(today.Year, 12, 31, 23, 59, 59);

            // Get revenue for different periods
            var todayRevenue = await _statisticRepository.GetRevenueByDateRangeAsync(today, tomorrow.AddSeconds(-1));
            var weekRevenue = await _statisticRepository.GetRevenueByDateRangeAsync(startOfWeek, endOfWeek);
            var monthRevenue = await _statisticRepository.GetRevenueByDateRangeAsync(startOfMonth, endOfMonth);
            var yearRevenue = await _statisticRepository.GetRevenueByDateRangeAsync(startOfYear, endOfYear);

            // Get order status counts
            var orderStatusCounts = await _statisticRepository.GetOrderStatusCountsAsync();

            // Get customer counts
            var totalCustomers = await _statisticRepository.GetTotalCustomersAsync();
            var newCustomers = await _statisticRepository.GetNewCustomersInRangeAsync(startOfMonth, endOfMonth);

            // Get top selling products
            var topProducts = await _statisticRepository.GetTopSellingProductsInRangeAsync(startOfMonth, endOfMonth, 5);

            // Get recent orders
            var recentOrders = await _statisticRepository.GetRecentOrdersAsync(5);

            return new DashboardSummaryResponse
            {
                TodayRevenue = todayRevenue.TotalRevenue,
                WeekRevenue = weekRevenue.TotalRevenue,
                MonthRevenue = monthRevenue.TotalRevenue,
                YearRevenue = yearRevenue.TotalRevenue,
                
                TodayOrders = todayRevenue.OrderCount,
                PendingOrders = orderStatusCounts.Pending,
                CompletedOrders = orderStatusCounts.Completed,
                
                TotalCustomers = totalCustomers,
                NewCustomers = newCustomers,
                
                TopSellingProducts = topProducts.Select(p => new ProductSaleDetail
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    QuantitySold = p.QuantitySold,
                    Revenue = p.Revenue,
                    ImageUrl = p.ImageUrl
                }).ToList(),
                
                RecentOrders = recentOrders.Select(o => new RecentOrder
                {
                    OrderId = o.OrderId,
                    CustomerName = o.CustomerName,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status
                }).ToList()
            };
        }
    }
} 