using AutoMapper;
using FluentValidation;
using FoodFlowSystem.DTOs;
using FoodFlowSystem.DTOs.Requests.Product;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Entities.Product;
using FoodFlowSystem.Entities.ProductVersions;
using FoodFlowSystem.Repositories.Feedback;
using FoodFlowSystem.Repositories.Product;
using FoodFlowSystem.Repositories.ProductVersion;
using FoodFlowSystem.Services.UploadImage;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace FoodFlowSystem.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductVersionRepository _productVersionRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        private readonly IValidator<UpdateProductRequest> _updateProductValidator;
        private readonly IValidator<CreateProductRequest> _createProductValidator;

        public ProductService(
            IProductRepository productRepository,
            IProductVersionRepository productVersionRepository,
            IFeedbackRepository feedbackRepository,
            ICloudinaryService cloudinaryService,
            IMapper mapper,
            ILogger<ProductService> logger,
            IValidator<UpdateProductRequest> updateProductValidator,
            IValidator<CreateProductRequest> createProductValidator
            )
        {
            _productRepository = productRepository;
            _productVersionRepository = productVersionRepository;
            _feedbackRepository = feedbackRepository;
            _cloudinaryService = cloudinaryService;
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
                var errors = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                });
                throw new ApiException("Đầu vào không hợp lệ: ", 400, errors);
            }

            var checkProduct = await _productRepository.IsExistProductNameAsync(request.Name);
            if (checkProduct != null)
            {
                throw new ApiException("Tên sản phẩm đã tồn tại. Vui lòng chọn tên khác.", 400);
            }

            var product = _mapper.Map<ProductEntity>(request);
            if (request.ImageFiles != null)
            {
                var image = await _cloudinaryService.UploadImageAsync(request.ImageFiles);
                product.ImageUrl = image;
            }

            var newProduct = await _productRepository.AddAsync(product);

            var productVersion = new ProductVersionEntity
            {
                ProductID = newProduct.ID,
                Price = request.Price,
                EffectiveDate = DateTime.UtcNow,
                IsActive = true,
            };

            var newProductVersion = await _productVersionRepository.AddAsync(productVersion);

            _logger.LogInformation("Product added successfully");

            var result = _mapper.Map<ProductResponse>(newProduct);
            result.Price = newProductVersion.Price;

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            var checkProduct = await _productRepository.GetProductById(id);
            if (checkProduct == null)
            {
                throw new ApiException("Product not found", 404);
            }

            var productVersion = await _productVersionRepository.GetLastProductVersionByProductIdAsync(id);
            productVersion.IsActive = false;

            await _productVersionRepository.UpdateAsync(productVersion);

            _logger.LogInformation("Product deleted successfully");
        }

        public async Task<IEnumerable<ProductResponse>> GetAllAsync()
        {
            var list = await _productRepository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<ProductResponse>>(list);
            if (!result.Any())
            {
                _logger.LogInformation("No products found");
                return result;
            }

            foreach (var product in result)
            {
                var lastProductVersion = await _productVersionRepository.GetLastProductVersionByProductIdAsync(product.Id);
                var feedback = await _feedbackRepository.GetAverageRateAndTotalFeedbacksByProductIdAsync(product.Id);
                product.Price = lastProductVersion.Price;
                product.AverageRated = feedback?.AverageRated ?? 0;
            }

            _logger.LogInformation("Get all products successfully");

            return result;
        }

        public async Task<IEnumerable<ProductResponse>> GetAllActiveAsync(string filter)
        {
            // filter dạng json
            JObject filterJson = string.IsNullOrEmpty(filter) ? new JObject() : JObject.Parse(filter);

            //string quickFilter = filterJson["quickFilter"]?.ToString();
            //quickFilter = string.IsNullOrWhiteSpace(quickFilter) ? null : quickFilter;

            string category = filterJson["category"]?.ToString();
            category = string.IsNullOrWhiteSpace(category) ? null : category;

            JArray priceRange = filterJson["priceRange"] as JArray;
            decimal minPrice = 0;
            decimal maxPrice = 0;
            if (priceRange != null && priceRange.Count >= 2)
            {
                decimal tempMin = priceRange[0].ToObject<decimal>();
                decimal tempMax = priceRange[1].ToObject<decimal>();
                if (tempMin != 0 || tempMax != 0)
                {
                    minPrice = tempMin;
                    maxPrice = tempMax;
                }
            }

            //float rating = 4;
            //string ratingStr = filterJson["rating"]?.ToString();
            //if (!string.IsNullOrWhiteSpace(ratingStr) && float.TryParse(ratingStr, out float parsedRating))
            //{
            //    rating = parsedRating;
            //}

            string sortBy = filterJson["sort"]?.ToString();
            sortBy = string.IsNullOrWhiteSpace(sortBy) ? null : sortBy;


            var list = await _productRepository.GetAllActiceAsync(category, minPrice, maxPrice, sortBy);
            var result = _mapper.Map<IEnumerable<ProductResponse>>(list);
            if (!result.Any())
            {
                _logger.LogInformation("No products found");
                return result;
            }

            foreach (var product in result)
            {
                var lastProductVersion = await _productVersionRepository.GetLastProductVersionByProductIdAsync(product.Id);
                var feedback = await _feedbackRepository.GetAverageRateAndTotalFeedbacksByProductIdAsync(product.Id);
                product.Price = lastProductVersion.Price;
                product.AverageRated = feedback?.AverageRated ?? 0;
            }

            _logger.LogInformation("Get all active products successfully");
            result = result.OrderByDescending(x => x.AverageRated).ToList();

            return result;
        }

        public async Task<int> CountAllActive()
        {
            var count = await _productRepository.CountActive();
            if (count == 0)
            {
                _logger.LogInformation("No products found");
                return 0;
            }

            _logger.LogInformation("Count all active products successfully");

            return count;
        }

        public async Task<ProductResponse> UpdateAsync(UpdateProductRequest request)
        {
            var validationResult = await _updateProductValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                });
                throw new ApiException("Invalid Input", 400, errors);
            }

            var product = await _productRepository.GetProductById(request.ID);
            if (product == null)
            {
                throw new ApiException("Product not found", 404);
            }

            var lastProductVersion = await _productVersionRepository.GetLastProductVersionByProductIdAsync(request.ID);

            if (lastProductVersion.Price != request.Price && request.Price > 0)
            {
                lastProductVersion.IsActive = false;
                await _productVersionRepository.UpdateAsync(lastProductVersion);

                var newProductVersion = new ProductVersionEntity
                {
                    ProductID = request.ID,
                    Price = request.Price,
                    EffectiveDate = DateTime.UtcNow,
                    IsActive = true,
                };
                await _productVersionRepository.AddAsync(newProductVersion);
            }

            var productDto = _mapper.Map<ProductEntity>(request);
            var productUpdated = await _productRepository.UpdateAsync(productDto);

            _logger.LogInformation("Product updated successfully");

            var result = _mapper.Map<ProductResponse>(productUpdated);
            result.Price = request.Price;

            return result;
        }

        public async Task<ProductResponse> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                throw new ApiException("Product not found", 404);
            }

            var result = _mapper.Map<ProductResponse>(product);

            var lastProductVersion = await _productVersionRepository.GetLastProductVersionByProductIdAsync(product.ID);
            var feedback = await _feedbackRepository.GetAverageRateAndTotalFeedbacksByProductIdAsync(product.ID);

            result.AverageRated = feedback?.AverageRated ?? 0;
            result.Price = lastProductVersion.Price;

            _logger.LogInformation("Get product by id successfully: ", id);

            return result;
        }
    }
}
