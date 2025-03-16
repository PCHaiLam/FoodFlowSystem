using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities.Product;
using Microsoft.EntityFrameworkCore;

namespace FoodFlowSystem.Repositories.Product
{
    public class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
    {
        public ProductRepository(MssqlDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<IEnumerable<ProductEntity>> GetByNameAsync(string name)
        {
            return await _dbContext.Products.Where(x => x.Name.Contains(name)).ToListAsync();
        }

        public async Task<ProductEntity> GetProductById(int input)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(x => x.ID == input);
        }

        public async Task<IEnumerable<ProductEntity>> GetByPriceAsync(decimal price)
        {
            return await _dbContext.Products
                            .Include(x => x.ProductVersions)
                            .Where(x => x.ProductVersions.Any(x => x.Price == price)).ToListAsync();
        }

        public Task<ProductEntity> IsExistProductNameAsync(string input)
        {
            return _dbContext.Products.FirstOrDefaultAsync(x => x.Name == input);
        }
    }
}
