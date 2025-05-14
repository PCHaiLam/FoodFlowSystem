using FoodFlowSystem.Entities.Product;

namespace FoodFlowSystem.Repositories.Product
{
    public interface IProductRepository : IBaseRepository<ProductEntity>
    {
        Task<ProductEntity> GetProductById(int id);
        Task<ProductEntity> IsExistProductNameAsync(string input);
        Task<IEnumerable<ProductEntity>> GetAllActiceAsync(string category, decimal minPrice, decimal maxPrice, string sort);
        Task<int> CountActive();
    }
}
