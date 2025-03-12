using FoodFlowSystem.Entities.Product;

namespace FoodFlowSystem.Repositories.Product
{
    public interface IProductRepository : IBaseRepository<ProductEntity>
    {
        Task<ProductEntity> IsExistProductAsync(string input);
        Task<IEnumerable<ProductEntity>> GetByNameAsync(string name);
        Task<IEnumerable<ProductEntity>> GetByPriceAsync(decimal price);
    }
}
