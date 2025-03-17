using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities.Category;

namespace FoodFlowSystem.Repositories.Category
{
    public class CategoryRepository : BaseRepository<CategoryEntity>, ICategoryRepository
    {
        public CategoryRepository(MssqlDbContext dbContext) : base(dbContext)
        {
        }
    }
}
