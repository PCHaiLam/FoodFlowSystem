using FoodFlowSystem.Entities.Product;

namespace FoodFlowSystem.Entities.ProductVersions
{
    public class ProductVersionEntity : BaseEntity
    {
        public decimal Price { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool IsActive { get; set; }
        public int ProductID { get; set; }
        public ProductEntity Product { get; set; }
    }
}
