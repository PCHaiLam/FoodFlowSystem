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

        public async Task<ProductEntity> IsExistProductAsync(string input)
        {
            if (int.TryParse(input, out int id))
            {
                return await _dbContext.Products.FirstOrDefaultAsync(x => x.ID == id);
            }

            return await _dbContext.Products.FirstOrDefaultAsync(x => x.Name == input);

        }

        public async Task<IEnumerable<ProductEntity>> GetByPriceAsync(decimal price)
        {
            return await _dbContext.Products
                            .Where(x => x.Price == price || x.Price <= price)
                            .OrderByDescending(x => x.Price)
                            .ToListAsync();
        }
    }
}
