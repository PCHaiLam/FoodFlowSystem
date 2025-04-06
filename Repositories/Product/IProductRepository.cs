using FoodFlowSystem.Entities.Product;

namespace FoodFlowSystem.Repositories.Product
{
    public interface IProductRepository : IBaseRepository<ProductEntity>
    {
        Task<ProductEntity> GetProductById(int id);
        Task<ProductEntity> IsExistProductNameAsync(string input);
        Task<IEnumerable<ProductEntity>> GetByNameAsync(string name);
        Task<IEnumerable<ProductEntity>> GetByPriceAsync(decimal price);
        Task<IEnumerable<ProductEntity>> GetAllActiceAsync(int page, int size);
        Task<int> CountActive();
    }
}
