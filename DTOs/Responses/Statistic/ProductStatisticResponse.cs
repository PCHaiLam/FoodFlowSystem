using System;
using System.Collections.Generic;

namespace FoodFlowSystem.DTOs.Responses.Statistic
{
    public class ProductStatisticResponse
    {
        public int TotalProductsSold { get; set; }
        public List<ProductSaleDetail> TopSellingProducts { get; set; } = new List<ProductSaleDetail>();
        public List<CategorySaleDetail> SalesByCategory { get; set; } = new List<CategorySaleDetail>();
        public string Period { get; set; }
    }

    public class ProductSaleDetail
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CategorySaleDetail
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
    }
} 