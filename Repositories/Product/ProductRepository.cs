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

        public async Task<IEnumerable<ProductEntity>> GetAllActiceAsync(int page, int pageSize, string filter, string search, string category, int? minPrice, int? maxPrice, string rating, string sort)
        {
            var result = new List<ProductEntity>();
            var query = _dbContext.Products
                        .Include(x => x.ProductVersions)
                        .Where(x => x.ProductVersions.Any(x => x.IsActive == true));

            //filter = "all" hoặc các điều kiện (search, category, minPrice, maxPrice, rating, sort) == null --> skip.take.tolistasync
            if (filter == "all" 
                || (string.IsNullOrEmpty(search) 
                && string.IsNullOrEmpty(category) 
                && minPrice == null 
                && maxPrice == null
                && string.IsNullOrEmpty(rating)
                && string.IsNullOrEmpty(sort)
                ))
            {
                
                result = await query
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                return result;
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.Contains(search));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(x => x.Category.Name.Contains(category));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(x => x.ProductVersions.Any(x => x.Price >= minPrice.Value));
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(x => x.ProductVersions.Any(x => x.Price <= maxPrice.Value));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "asc":
                        query = query.OrderBy(x => x.Name);
                        break;
                    case "desc":
                        query = query.OrderByDescending(x => x.Name);
                        break;
                    default:
                        break;
                }
            }

            result = await query.ToListAsync();
            return result;
        }

        public async Task<int> CountActive()
        {
            return await _dbContext.Products
                        .Include(x => x.ProductVersions)
                        .Where(x => x.ProductVersions.Any(x => x.IsActive == true))
                        .CountAsync();
        }
    }
}
