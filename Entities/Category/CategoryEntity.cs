using FoodFlowSystem.Entities.Product;

namespace FoodFlowSystem.Entities.Category
{
    public class CategoryEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ProductEntity> Products { get; set; }
    }
}
