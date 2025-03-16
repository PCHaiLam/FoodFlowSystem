using AutoMapper;
using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Product;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Entities.Product;
using FoodFlowSystem.Middlewares.Exceptions;
using FoodFlowSystem.Repositories.Product;

namespace FoodFlowSystem.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        private readonly IValidator<UpdateProductRequest> _updateProductValidator;
        private readonly IValidator<CreateProductRequest> _createProductValidator;

        public ProductService(
            IProductRepository productRepository,
            IMapper mapper,
            ILogger<ProductService> logger,
            IValidator<UpdateProductRequest> updateProductValidator,
            IValidator<CreateProductRequest> createProductValidator
            )
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
            _updateProductValidator = updateProductValidator;
            _createProductValidator = createProductValidator;
        }

        public async Task<ProductResponse> AddAsync(CreateProductRequest request)
        {
            var validationResult = await _createProductValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogError("Validation failed");
                throw new ApiException("Invalid input", 400);
            }

            var checkProduct = _productRepository.IsExistProductNameAsync(request.Name);
            if (checkProduct != null)
            {
                   _logger.LogError("Product already exists");
                    throw new ApiException("Product already exists", 400);
            }

            var product = _mapper.Map<ProductEntity>(request);
            var result = await _productRepository.AddAsync(product);

            _logger.LogInformation("Product added successfully");

            return _mapper.Map<ProductResponse>(result);
        }

        public async Task DeleteAsync(int id)
        {
            var checkProduct = await _productRepository.GetProductById(id);
            if (checkProduct == null)
            {
                _logger.LogError("Product not found");
                throw new ApiException("Product not found", 404);
            }

            await _productRepository.DeleteAsync(id);

            _logger.LogInformation("Product deleted successfully");
        }

        public async Task<IEnumerable<ProductResponse>> GetAllAsync()
        {
            var list = await _productRepository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<ProductResponse>>(list);   

            _logger.LogInformation("Get all products successfully");

            return result;

        }

        public async Task<IEnumerable<ProductResponse>> GetByNameAsync(string name)
        {
            var list = await _productRepository.GetByNameAsync(name);
            var result = _mapper.Map<IEnumerable<ProductResponse>>(list);

            _logger.LogInformation($"Get product by name successfully: {name}");

            return result;
        }

        public async Task<IEnumerable<ProductResponse>> GetByPriceAsync(decimal price)
        {
            var list = await _productRepository.GetByPriceAsync(price);
            var result = _mapper.Map<IEnumerable<ProductResponse>>(list);

            _logger.LogInformation($"Get product by price successfully: {price}");

            return result;
        }

        public async Task<ProductResponse> UpdateAsync(UpdateProductRequest request)
        {
            var validationResult = await _updateProductValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogError("Invalid Input");
                throw new ApiException("Invalid Input", 400);
            }

            var product = await _productRepository.GetProductById(request.ID);
            if (product == null)
            {
                _logger.LogError("Product not found");
                throw new ApiException("Product not found", 404);
            }

            var productDto = _mapper.Map<ProductEntity>(request);
            var result = await _productRepository.UpdateAsync(productDto);

            _logger.LogInformation("Product updated successfully");

            return _mapper.Map<ProductResponse>(result);
        }
    }
}
