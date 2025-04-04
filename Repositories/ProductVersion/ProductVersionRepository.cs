using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities.ProductVersions;
using Microsoft.EntityFrameworkCore;

namespace FoodFlowSystem.Repositories.ProductVersion
{
    public class ProductVersionRepository : BaseRepository<ProductVersionEntity>, IProductVersionRepository
    {
        public ProductVersionRepository(MssqlDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ProductVersionEntity> GetLastProductVersionByProductIdAsync(int id)
        {
            var result = await _dbContext.ProductVersions
                .Where(x => x.ProductID == id)
                .OrderByDescending(x => x.ID)
                .FirstOrDefaultAsync();
            return result;
        }
    }
}
