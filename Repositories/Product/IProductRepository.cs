using FoodFlowSystem.Entities.Product;

namespace FoodFlowSystem.Repositories.Product
{
    public interface IProductRepository : IBaseRepository<ProductEntity>
    {
        Task<ProductEntity> GetProductById(int input);
        Task<ProductEntity> IsExistProductNameAsync(string input);
        Task<IEnumerable<ProductEntity>> GetByNameAsync(string name);
        Task<IEnumerable<ProductEntity>> GetByPriceAsync(decimal price);
    }
}
