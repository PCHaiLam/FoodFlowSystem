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
            var result = _dbContext.Products.FirstOrDefaultAsync(x => x.Name == input);
            return result;
        }

        public async Task<IEnumerable<ProductEntity>> GetAllActiceAsync(string category, decimal minPrice, decimal maxPrice, string sort)
        {
            var query = _dbContext.Products
                        .Include(x => x.Category)
                        .Include(x => x.ProductVersions)
                        .Where(x => x.ProductVersions.Any(x => x.IsActive == true));

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(x => x.Category.Name.Contains(category));
            }

            if (minPrice > 0)
            {
                query = query.Where(x => x.ProductVersions.Any(x => x.Price >= minPrice));
            }

            if (maxPrice > 0)
            {
                query = query.Where(x => x.ProductVersions.Any(x => x.Price <= maxPrice));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "asc":
                        query = query.OrderBy(x => x.ProductVersions.FirstOrDefault().Price);
                        break;
                    case "desc":
                        query = query.OrderByDescending(x => x.ProductVersions.FirstOrDefault().Price);
                        break;
                    default:
                        break;
                }
            }

            return await query.ToListAsync();
        }

        public async Task<int> CountActive()
        {
            return await _dbContext.Products
                        .Include(x => x.ProductVersions)
                        .Where(x => x.ProductVersions.Any(x => x.IsActive == true))
                        .CountAsync();
        }

        public async Task<decimal> GetProductPriceAsync(int productId)
        {
            var product = await _dbContext.Products
                        .Include(x => x.ProductVersions)
                        .FirstOrDefaultAsync(x => x.ID == productId);

            if (product != null && product.ProductVersions.Any())
            {
                return product.ProductVersions.FirstOrDefault().Price;
            }

            return 0;
        }
    }
}
