namespace FoodFlowSystem.DTOs.Requests.Product
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string UrlImage { get; set; }
        public int CategoryID { get; set; }
    }
}
