using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using AutoMapper;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.DTOs.Responses.Statistic;
using FoodFlowSystem.Repositories.Invoice;
using FoodFlowSystem.Repositories.OrderItem;
using FoodFlowSystem.Repositories.Product;

namespace FoodFlowSystem.Services.Statistic
{
    public class StatisticService : IStatisticService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;

        public StatisticService(
            IOrderItemRepository orderItemRepository,
            IProductRepository productRepository,
            IInvoiceRepository invoiceRepository,
            IMapper mapper
            )
        {
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _invoiceRepository = invoiceRepository;
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
                DailyRevenue = await GetDailyStatisticAsync(),
                ProductStatistics = listOrderItems,
                InvoicesStatistic = _mapper.Map<ICollection<InvoiceResponse>>(listInvoices),
            };

            return result;
        }

        public async Task<decimal> GetDailyStatisticAsync()
        {
            var startDate = DateTime.Now;
            var listInvoices = await _invoiceRepository.GetByArangeDate(startDate, startDate);

            var result = listInvoices.Sum(x => x.TotalAmount);

            return result;
        }
    }
} 