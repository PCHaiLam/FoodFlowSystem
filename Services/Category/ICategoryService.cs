using FoodFlowSystem.DTOs.Requests.Category;
using FoodFlowSystem.DTOs.Responses;

namespace FoodFlowSystem.Services.Category
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetCategoriesAsync();
        Task<CategoryResponse> GetCategoryByIdAsync(int id);
        Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request);
        Task<CategoryResponse> UpdateCategoryAsync(UpdateCategoryRequest request);
        Task DeleteCategoryAsync(int id);
    }
}
