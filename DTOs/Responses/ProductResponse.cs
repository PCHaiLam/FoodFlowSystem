﻿namespace FoodFlowSystem.DTOs.Responses
{
    public class ProductResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
