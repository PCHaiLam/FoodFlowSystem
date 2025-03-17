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
            return await _dbContext.ProductVersions
                .LastOrDefaultAsync(x => x.ID == id);
        }
    }
}
