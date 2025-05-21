using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using AutoMapper;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.DTOs.Responses.Statistic;
using FoodFlowSystem.Repositories.Feedback;
using FoodFlowSystem.Repositories.Invoice;
using FoodFlowSystem.Repositories.OrderItem;
using FoodFlowSystem.Repositories.Product;
using FoodFlowSystem.Repositories.User;

namespace FoodFlowSystem.Services.Statistic
{
    public class StatisticService : IStatisticService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public StatisticService(
            IOrderItemRepository orderItemRepository,
            IProductRepository productRepository,
            IInvoiceRepository invoiceRepository,
            IFeedbackRepository feedbackRepository,
            IUserRepository userRepository,
            IMapper mapper
            )
        {
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _invoiceRepository = invoiceRepository;
            _feedbackRepository = feedbackRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        
        public async Task<DailyStatisticResponse> GetStatisticByArangeDateAsync(DateTime startDate, DateTime endDate)
        {
            var listInvoices = await _invoiceRepository.GetByArangeDate(startDate, endDate);
            var listOrderItems = await _orderItemRepository.GetByArangeDateAsync(startDate, endDate);

            foreach (var orderItem in listOrderItems)
            {
                orderItem.UnitPrice = await _productRepository.GetProductPriceAsync(orderItem.ProductId);
            }

            var result = new DailyStatisticResponse
            {
                DailyRevenue = await GetDailyRevenueStatisticAsync(),
                TotalSold = await GetDailyProductSoldStatisticAsync(),
                TotalReviews = await GetDailyReviewStatisticAsync(),
                TotalCustomers = await GetDailyUserStatisticAsync(),
                ProductStatistics = listOrderItems,
                InvoicesStatistic = _mapper.Map<ICollection<InvoiceResponse>>(listInvoices),
            };

            return result;
        }

        private async Task<decimal> GetDailyRevenueStatisticAsync()
        {
            var today = DateTime.Today;
            var startDate = today;
            var endDate = today.AddDays(1).AddTicks(-1);

            var listInvoices = await _invoiceRepository.GetByArangeDate(startDate, endDate);

            if (listInvoices == null || !listInvoices.Any())
            {
                return 0m;
            }

            var result = listInvoices.Where(x => x.Order.Status == "Completed").Sum(x => x.TotalAmount);

            return result;
        }

        private async Task<int> GetDailyProductSoldStatisticAsync()
        {
            var today = DateTime.Today;
            var startDate = today;
            var endDate = today.AddDays(1).AddTicks(-1);

            var listProductSold = await _orderItemRepository.GetByArangeDateAsync(startDate, endDate);

            if (listProductSold == null || !listProductSold.Any())
            {
                return 0;
            }

            var result = listProductSold.Sum(x => x.QuantitySold);

            return result;
        }

        private async Task<int> GetDailyReviewStatisticAsync()
        {
            var today = DateTime.Today;
            var startDate = today;
            var endDate = today.AddDays(1).AddTicks(-1);

            var listReviews = await _feedbackRepository.GetByArangeDateAsync(startDate, endDate);

            if (listReviews == null || !listReviews.Any())
            {
                return 0;
            }

            var result = listReviews.Count();

            return result;
        }

        private async Task<int> GetDailyUserStatisticAsync()
        {
            var today = DateTime.Today;
            var startDate = today;
            var endDate = today.AddDays(1).AddTicks(-1);

            var listUsers = await _userRepository.GetByArangeDateAsync(startDate, endDate);

            if (listUsers == null || !listUsers.Any())
            {
                return 0;
            }

            var result = listUsers.Count();

            return result;  
        }
    }
} 