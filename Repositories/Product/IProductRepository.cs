using FoodFlowSystem.Entities.Invoice;
using FoodFlowSystem.Entities.Product;

namespace FoodFlowSystem.Repositories.Product
{
    public interface IProductRepository : IBaseRepository<ProductEntity>
    {
        Task<int> CountActive();
        Task<ProductEntity> GetProductById(int id);
        Task<decimal> GetProductPriceAsync(int productId);
        Task<ProductEntity> IsExistProductNameAsync(string input);
        Task<IEnumerable<ProductEntity>> GetAllActiceAsync(string category, decimal minPrice, decimal maxPrice, string sort);
        //Task<ICollection<ProductEntity>> GetByArangeDate(DateTime startDate, DateTime endDate);
    }
}
