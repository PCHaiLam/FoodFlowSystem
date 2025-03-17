using AutoMapper;
using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Category;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Entities.Category;
using FoodFlowSystem.Middlewares.Exceptions;
using FoodFlowSystem.Repositories.Category;

namespace FoodFlowSystem.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        private readonly IValidator<UpdateCategoryRequest> _updateCategoryValidator;
        private readonly IValidator<CreateCategoryRequest> _createCategoryValidator;

        public CategoryService(
            ILogger<CategoryService> logger
            , ICategoryRepository categoryRepository
            , IMapper mapper
            , IValidator<UpdateCategoryRequest> updateCategoryValidator
            , IValidator<CreateCategoryRequest> createCategoryValidator
            )
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _updateCategoryValidator = updateCategoryValidator;
            _createCategoryValidator = createCategoryValidator;
        }

        public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request)
        {
            var validationResult = await _createCategoryValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                _logger.LogError("Validation error");
                throw new ApiException("Invalid input.", 400);
            }

            var category = _mapper.Map<CategoryEntity>(request);

            await _categoryRepository.AddAsync(category);

            _logger.LogInformation("Category created successfully");

            var result = _mapper.Map<CategoryResponse>(category);
            return result;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var checkCategory = await _categoryRepository.GetByIdAsync(id);

            if (checkCategory == null)
            {
                _logger.LogError("Category not found");
                throw new ApiException("Category not found", 404);
            }

            await _categoryRepository.DeleteAsync(id);

            _logger.LogInformation("Category deleted successfully");
        }

        public async Task<IEnumerable<CategoryResponse>> GetCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            var result = _mapper.Map<IEnumerable<CategoryResponse>>(categories);

            _logger.LogInformation("Categories listed successfully");

            return result;
        }

        public async Task<CategoryResponse> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                _logger.LogError("Category not found");
                throw new ApiException("Category not found", 404);
            }

            var result = _mapper.Map<CategoryResponse>(category);

            _logger.LogInformation("Category listed successfully");

            return result;
        }

        public async Task<CategoryResponse> UpdateCategoryAsync(UpdateCategoryRequest request)
        {
            var validationResult = await _updateCategoryValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                _logger.LogError("Validation error");
                throw new ApiException("Invalid input.", 400);
            }

            var category = await _categoryRepository.GetByIdAsync(request.Id);

            if (category == null)
            {
                _logger.LogError("Category not found");
                throw new ApiException("Category not found", 404);
            }

            category.Name = request.Name;

            await _categoryRepository.UpdateAsync(category);

            _logger.LogInformation("Category updated successfully");

            var result = _mapper.Map<CategoryResponse>(category);
            return result;
        }
    }
}
