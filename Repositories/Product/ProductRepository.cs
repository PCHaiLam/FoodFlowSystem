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
            return await _dbContext.Products
                        .Include(x => x.ProductVersions)
                        .Where(x => x.Name.Contains(name) && x.ProductVersions.Any(x => x.IsActive == true))
                        .ToListAsync();
        }

        public async Task<ProductEntity> GetProductById(int id)
        {
            return await _dbContext.Products
                        .Include(x => x.ProductVersions)
                        .FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<IEnumerable<ProductEntity>> GetByPriceAsync(decimal price)
        {
            return await _dbContext.Products
                        .Include(x => x.ProductVersions)
                        .Where(x => x.ProductVersions.Any(x => x.Price == price && x.IsActive == true))
                        .ToListAsync();
        }

        public Task<ProductEntity> IsExistProductNameAsync(string input)
        {
            return _dbContext.Products.FirstOrDefaultAsync(x => x.Name == input);
        }

        public async Task<IEnumerable<ProductEntity>> GetAllActiceAsync()
        {
            return await _dbContext.Products
                            .Include(x => x.ProductVersions)
                            .Where(x => x.ProductVersions.Any(x => x.IsActive == true))
                            .ToListAsync();
        }
    }
}
