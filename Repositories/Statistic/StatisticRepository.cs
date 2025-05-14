using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodFlowSystem.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace FoodFlowSystem.Repositories.Statistic
{
    public class StatisticRepository : IStatisticRepository
    {
        private readonly MssqlDbContext _context;

        public StatisticRepository(MssqlDbContext context)
        {
            _context = context;
        }

        public async Task<(decimal TotalRevenue, int OrderCount)> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _context.Orders
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate && o.Status != "Cancelled")
                .ToListAsync();

            return (
                TotalRevenue: orders.Sum(o => o.TotalAmount),
                OrderCount: orders.Count
            );
        }

        public async Task<List<(DateTime Date, decimal Revenue, int OrderCount)>> GetDailyRevenueInRangeAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _context.Orders
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate && o.Status != "Cancelled")
                .ToListAsync();

            return orders
                .GroupBy(o => o.CreatedAt.Date)
                .Select(g => (
                    Date: g.Key,
                    Revenue: g.Sum(o => o.TotalAmount),
                    OrderCount: g.Count()
                ))
                .OrderBy(x => x.Date)
                .ToList();
        }

        public async Task<List<(int ProductId, string ProductName, int QuantitySold, decimal Revenue, string ImageUrl)>> GetTopSellingProductsInRangeAsync(DateTime startDate, DateTime endDate, int limit = 10)
        {
            var result = await _context.OrderItems
                .Where(oi => oi.Order.CreatedAt >= startDate && oi.Order.CreatedAt <= endDate && oi.Order.Status != "Cancelled")
                .Join(_context.Products, oi => oi.ProductId, p => p.Id, (oi, p) => new { OrderItem = oi, Product = p })
                .GroupBy(x => new { x.Product.Id, x.Product.Name, x.Product.ImageUrl })
                .Select(g => new
                {
                    ProductId = g.Key.Id,
                    ProductName = g.Key.Name,
                    QuantitySold = g.Sum(x => x.OrderItem.Quantity),
                    Revenue = g.Sum(x => x.OrderItem.Price * x.OrderItem.Quantity),
                    ImageUrl = g.Key.ImageUrl
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(limit)
                .ToListAsync();

            return result.Select(x => (
                x.ProductId,
                x.ProductName,
                x.QuantitySold,
                x.Revenue,
                x.ImageUrl
            )).ToList();
        }

        public async Task<List<(int CategoryId, string CategoryName, int QuantitySold, decimal Revenue)>> GetCategorySalesInRangeAsync(DateTime startDate, DateTime endDate)
        {
            var result = await _context.OrderItems
                .Where(oi => oi.Order.CreatedAt >= startDate && oi.Order.CreatedAt <= endDate && oi.Order.Status != "Cancelled")
                .Join(_context.Products, oi => oi.ProductId, p => p.Id, (oi, p) => new { OrderItem = oi, Product = p })
                .Join(_context.Categories, x => x.Product.CategoryId, c => c.Id, (x, c) => new { x.OrderItem, x.Product, Category = c })
                .GroupBy(x => new { x.Category.Id, x.Category.Name })
                .Select(g => new
                {
                    CategoryId = g.Key.Id,
                    CategoryName = g.Key.Name,
                    QuantitySold = g.Sum(x => x.OrderItem.Quantity),
                    Revenue = g.Sum(x => x.OrderItem.Price * x.OrderItem.Quantity)
                })
                .OrderByDescending(x => x.QuantitySold)
                .ToListAsync();

            return result.Select(x => (
                x.CategoryId,
                x.CategoryName,
                x.QuantitySold,
                x.Revenue
            )).ToList();
        }

        public async Task<int> GetTotalCustomersAsync()
        {
            return await _context.Users
                .Where(u => u.RoleId == 2)  // Assuming RoleId 2 is for customers
                .CountAsync();
        }

        public async Task<int> GetNewCustomersInRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Users
                .Where(u => u.RoleId == 2 && u.CreatedAt >= startDate && u.CreatedAt <= endDate)
                .CountAsync();
        }

        public async Task<(int Pending, int Completed)> GetOrderStatusCountsAsync()
        {
            var pendingCount = await _context.Orders
                .Where(o => o.Status == "Pending" || o.Status == "Processing")
                .CountAsync();

            var completedCount = await _context.Orders
                .Where(o => o.Status == "Completed" || o.Status == "Delivered")
                .CountAsync();

            return (pendingCount, completedCount);
        }

        public async Task<List<(int OrderId, string CustomerName, DateTime OrderDate, decimal TotalAmount, string Status)>> GetRecentOrdersAsync(int limit = 5)
        {
            var recentOrders = await _context.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .Take(limit)
                .Select(o => new
                {
                    OrderId = o.Id,
                    CustomerName = o.User.FullName,
                    OrderDate = o.CreatedAt,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status
                })
                .ToListAsync();

            return recentOrders.Select(o => (
                o.OrderId,
                o.CustomerName,
                o.OrderDate,
                o.TotalAmount,
                o.Status
            )).ToList();
        }
    }
} 