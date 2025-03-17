using FoodFlowSystem.Entities.ProductVersions;

namespace FoodFlowSystem.Repositories.ProductVersion
{
    public interface IProductVersionRepository : IBaseRepository<ProductVersionEntity>
    {
        Task<ProductVersionEntity> GetLastProductVersionByProductIdAsync(int id);
    }
}
