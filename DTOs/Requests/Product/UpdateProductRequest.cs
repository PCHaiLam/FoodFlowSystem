namespace FoodFlowSystem.DTOs.Requests.Product
{
    public class UpdateProductRequest
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string UrlImage { get; set; }
        public int CategoryID { get; set; }
    }
}
