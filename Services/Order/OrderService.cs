using AutoMapper;
using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Order;
using FoodFlowSystem.DTOs.Requests.OrderItem;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Entities.Order;
using FoodFlowSystem.Entities.OrderItem;
using FoodFlowSystem.Middlewares.Exceptions;
using FoodFlowSystem.Repositories.Order;
using FoodFlowSystem.Repositories.OrderItem;
using FoodFlowSystem.Repositories.Product;

namespace FoodFlowSystem.Services.Order
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;
        private readonly IValidator<CreateOrderRequest> _createOrderValidator;
        private readonly IValidator<UpdateOrderRequest> _updateOrderValidator;
        private readonly IValidator<CreateOrderItemRequest> _createOrderItemValidator;
        private readonly IValidator<UpdateOrderItemRequest> _updateOrderItemValidator;

        public OrderService(
            IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IProductRepository productRepository,
            IMapper mapper,
            ILogger<OrderService> logger,
            IValidator<CreateOrderRequest> createOrderValidator,
            IValidator<UpdateOrderRequest> updateOrderValidator
            ) : base(httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
            _createOrderValidator = createOrderValidator;
            _updateOrderValidator = updateOrderValidator;
        }

        public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
        {
            var userId = this.GetCurrentUserId();
            var validationResult = await _createOrderValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                _logger.LogError("Validation create order request failed");
                var errors = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                });
                throw new ApiException("Invalid create order input.", 400, errors);
            }

            var order = _mapper.Map<OrderEntity>(request);
            order.UserID = userId;
            order.Status = "Pending";

            var newOrder = await _orderRepository.AddAsync(order);

            foreach (var item in request.OrderItems)
            {
                var validatorOrderItem = await _createOrderItemValidator.ValidateAsync(item);
                if (!validatorOrderItem.IsValid)
                {
                    _logger.LogError("Validation create order item request failed");
                    throw new ApiException("Invalid create order item input.");
                }

                var product = await _productRepository.GetByIdAsync(item.ProductID);
                var lastVersion = product.ProductVersions.LastOrDefault(x => x.IsActive == true);

                if (lastVersion == null)
                {
                    _logger.LogError("Product is inactive");
                    throw new ApiException("Product is inactive.", 400);
                }

                if (product == null)
                {
                    _logger.LogError("Product not found");
                    throw new ApiException("Product not found.", 404);
                }

                if (product.Quantity < item.Quantity)
                {
                    _logger.LogError("Product out of stock");
                    throw new ApiException($"Product out of stock. Remaining products: {item.Quantity}", 400);
                }

                var newOrderItem = _mapper.Map<OrderItemEntity>(item);

                newOrderItem.OrderID = newOrder.ID;
                newOrderItem.Price = lastVersion.Price;

                await _orderItemRepository.AddAsync(newOrderItem);

                product.Quantity -= item.Quantity;
                await _productRepository.UpdateAsync(product);
            }

            _logger.LogInformation("Order created successfully");

            var listOI = await _orderItemRepository.GetByOrderId(newOrder.ID);

            var result = _mapper.Map<OrderResponse>(newOrder);
            result.ListOrderItems = _mapper.Map<ICollection<OrderItemResponse>>(listOI);

            return _mapper.Map<OrderResponse>(result);
        }

        public async Task DeleteOrderAsync(int id)
        {
            var checkOrder = await _orderRepository.GetByIdAsync(id);

            if (checkOrder == null)
            {
                _logger.LogError("Order not found");
                throw new ApiException("Order not found", 404);
            }

            await _orderRepository.DeleteAsync(id);

            _logger.LogInformation("Order deleted successfully");
        }

        public async Task<ICollection<OrderResponse>> GetOrderByDate(DateTime date)
        {
            var orders = await _orderRepository.GetByDateAsync(date);

            if (orders == null)
            {
                _logger.LogError("Order not found");
                throw new ApiException("Order not found", 404);
            }

            var result = _mapper.Map<ICollection<OrderResponse>>(orders);

            _logger.LogInformation("Order listed successfully");

            return result;
        }

        public async Task<OrderResponse> GetOrderById(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                _logger.LogError("Order not found");
                throw new ApiException("Order not found", 404);
            }

            var listOI = await _orderItemRepository.GetByOrderId(order.ID);

            var result = _mapper.Map<OrderResponse>(order);
            result.ListOrderItems = _mapper.Map<ICollection<OrderItemResponse>>(listOI);

            _logger.LogInformation("Order listed successfully");

            return result;
        }

        public async Task<ICollection<OrderResponse>> GetOrderRangeDate(DateTime startDate, DateTime endDate)
        {
            var orders = await _orderRepository.GetByDateRangeAsync(startDate, endDate);

            if (orders == null)
            {
                _logger.LogError("Order not found");
                throw new ApiException("Order not found", 404);
            }

            var result = _mapper.Map<ICollection<OrderResponse>>(orders);

            _logger.LogInformation("Order listed successfully");

            return result;
        }

        public async Task<ICollection<OrderResponse>> GetOrdersByUserId(int id)
        {
            var orders = await _orderRepository.GetByUserIdAsync(id);

            if (orders == null)
            {
                _logger.LogError("Order not found");
                throw new ApiException("Order not found", 404);
            }

            var result = _mapper.Map<ICollection<OrderResponse>>(orders);

            _logger.LogInformation("Order listed successfully");

            return result;
        }

        public async Task<OrderResponse> UpdateOrderAsync(UpdateOrderRequest request)
        {
            var validationResult = await _updateOrderValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                _logger.LogError("Validation update order request failed");
                var errors = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                });
                throw new ApiException("Invalid update order input.", 400, errors);
            }

            var order = await _orderRepository.GetByIdAsync(request.OrderID);

            if (order == null)
            {
                _logger.LogError("Order not found");
                throw new ApiException("Order not found", 404);
            }

            foreach (var item in request.OrderItems)
            {
                var validatorOrderItem = await _updateOrderItemValidator.ValidateAsync(item);
                if (!validatorOrderItem.IsValid)
                {
                    _logger.LogError("Validation update order item request failed");
                    throw new ApiException("Invalid update order item input.");
                }

                var orderItem = await _orderItemRepository.GetByOrderIdAndProductId(order.ID, item.ProductID);
                var product = await _productRepository.GetByIdAsync(item.ProductID);
                var lastVersion = product.ProductVersions.LastOrDefault(x => x.IsActive == true);

                // If the quantity is 0, delete the order item
                if (orderItem.Quantity == 0)
                {
                    await _orderItemRepository.DeleteAsync(orderItem.ID);
                    break;
                }

                // If the order item is null, create a new one
                if (orderItem == null)
                {
                    var newOrderItem = _mapper.Map<OrderItemEntity>(item);
                    newOrderItem.OrderID = order.ID;
                    newOrderItem.Price = lastVersion.Price;

                    if (product.Quantity < item.Quantity)
                    {
                        _logger.LogError("Product out of stock");
                        throw new ApiException($"Product out of stock. Remaining products: {item.Quantity}", 400);
                    }

                    product.Quantity -= item.Quantity;
                    await _productRepository.UpdateAsync(product);

                    await _orderItemRepository.AddAsync(newOrderItem);
                } 
                // If the order item is not null, update the quantity
                else 
                {
                    var quantityDiff = item.Quantity - orderItem.Quantity;

                    if (quantityDiff > 0)
                    {
                        if (product.Quantity < quantityDiff)
                        {
                            _logger.LogError("Product out of stock");
                            throw new ApiException($"Product out of stock. Remaining products: {item.Quantity}", 400);
                        }

                        product.Quantity -= quantityDiff;
                    }
                    else if (quantityDiff < 0)
                    {
                        product.Quantity += Math.Abs(quantityDiff);
                    }

                    await _productRepository.UpdateAsync(product);

                    orderItem.Quantity = item.Quantity;

                    await _orderItemRepository.UpdateAsync(orderItem);
                }
            }

            var orderDto = _mapper.Map<OrderEntity>(request);

            var newOrder = await _orderRepository.UpdateAsync(orderDto);

            _logger.LogInformation("Order updated successfully");

            var listOI = await _orderItemRepository.GetByOrderId(order.ID);
            var result = _mapper.Map<OrderResponse>(newOrder);
            result.ListOrderItems = _mapper.Map<ICollection<OrderItemResponse>>(listOI);

            return result;
        }
    }
}
