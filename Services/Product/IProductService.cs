using FoodFlowSystem.DTOs.Requests.Product;
using FoodFlowSystem.DTOs.Responses;

namespace FoodFlowSystem.Services.Product
{
    public interface IProductService
    {
        Task<ProductResponse> GetByIdAsync(int id);
        Task<IEnumerable<ProductResponse>> GetAllAsync();
        Task<ProductResponse> AddAsync(CreateProductRequest request);
        Task<ProductResponse> UpdateAsync(UpdateProductRequest request);
        Task DeleteAsync(int id);
        Task<IEnumerable<ProductResponse>> GetByNameAsync(string name);
        Task<IEnumerable<ProductResponse>> GetByPriceAsync(decimal price);
        Task<IEnumerable<ProductResponse>> GetAllActiveAsync(string filter);
        Task<int> CountAllActive();
    }
}
