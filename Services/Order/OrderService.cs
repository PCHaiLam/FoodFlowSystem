using AutoMapper;
using FluentValidation;
using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.DTOs;
using FoodFlowSystem.DTOs.Requests.Order;
using FoodFlowSystem.DTOs.Requests.OrderItem;
using FoodFlowSystem.DTOs.Requests.Payment;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Entities.Invoice;
using FoodFlowSystem.Entities.Order;
using FoodFlowSystem.Entities.OrderItem;
using FoodFlowSystem.Entities.Payment;
using FoodFlowSystem.Extensions;
using FoodFlowSystem.Repositories.EmailTemplates;
using FoodFlowSystem.Repositories.Invoice;
using FoodFlowSystem.Repositories.Order;
using FoodFlowSystem.Repositories.OrderItem;
using FoodFlowSystem.Repositories.Payment;
using FoodFlowSystem.Repositories.Product;
using FoodFlowSystem.Repositories.ProductVersion;
using FoodFlowSystem.Repositories.User;
using FoodFlowSystem.Services.Payment;
using FoodFlowSystem.Services.SendMail;

namespace FoodFlowSystem.Services.Order
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly MssqlDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductVersionRepository _productVersionRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IVNPayService _vnpayService;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;
        private readonly IValidator<CreateOrderRequest> _createOrderValidator;
        private readonly IValidator<UpdateOrderRequest> _updateOrderValidator;
        private readonly IValidator<CreateOrderItemRequest> _createOrderItemValidator;
        private readonly IValidator<UpdateOrderItemRequest> _updateOrderItemValidator;

        public OrderService(
            IHttpContextAccessor httpContextAccessor,
            MssqlDbContext dbContext,
            IUserRepository userRepository,
            IInvoiceRepository invoiceRepository,
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IProductRepository productRepository,
            IProductVersionRepository productVersionRepository,
            IPaymentRepository paymentRepository,
            IVNPayService vNPayService,
            IMapper mapper,
            ILogger<OrderService> logger,
            IValidator<CreateOrderRequest> createOrderValidator,
            IValidator<UpdateOrderRequest> updateOrderValidator,
            IValidator<CreateOrderItemRequest> createOrderItemValidator,
            IValidator<UpdateOrderItemRequest> updateOrderItemValidator
            ) : base(httpContextAccessor)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _invoiceRepository = invoiceRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _productVersionRepository = productVersionRepository;
            _paymentRepository = paymentRepository;
            _vnpayService = vNPayService;
            _mapper = mapper;
            _logger = logger;
            _createOrderValidator = createOrderValidator;
            _updateOrderValidator = updateOrderValidator;
            _createOrderItemValidator = createOrderItemValidator;
            _updateOrderItemValidator = updateOrderItemValidator;
        }

        public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
        {
            var validationResult = await _createOrderValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                });
                throw new ApiException("Invalid create order input.", 400, errors);
            }

            // role: 1-admin, 2-customer, 3-staff
            var userRole = this.GetCurrentUserRole();

            string orderType = "in_restaurant";
            if (userRole == 2)
            {
                orderType = "online";
            }

            //2 case
            //1. order in restaurant
            if (orderType == "in_restaurant")
            {
                if (request.TableId == null || request.TableId == 0)
                {
                    throw new ApiException("Bàn không hợp lệ", 400);
                }

                if (request.OrderItems.Count == 0)
                {
                    throw new ApiException("Vui lòng chọn ít nhất một món ăn", 400);
                }
            }
            //2. order online
            else if (orderType == "online")
            {
                bool hasReservationDate = request.ReservationDate != null;
                bool hasReservationTime = request.ReservationTime != null;
                bool hasNumOfGuests = request.NumOfGuests > 0;
                bool hasOrderItems = request.OrderItems != null && request.OrderItems.Count > 0;

                if (!hasReservationTime)
                {
                    throw new ApiException("Thời gian đặt bàn không hợp lệ", 400);
                }

                if (!hasReservationDate)
                {
                    throw new ApiException("Ngày đặt bàn không hợp lệ", 400);
                }

                if (!hasNumOfGuests)
                {
                    throw new ApiException("Số lượng khách không hợp lệ", 400);
                }

                if (!hasOrderItems)
                {
                    throw new ApiException("Vui lòng chọn món hoặc đặt bàn", 400);
                }
            }

            var userId = this.GetCurrentUserId();
            var user = await _userRepository.GetByIdAsync(userId);
            user.Phone = request.Phone;
            _userRepository.UpdateWithoutSaving(user);

            var order = _mapper.Map<OrderEntity>(request);
            order.TableID = request.TableId == null || request.TableId == 0 ? null : request.TableId;
            order.UserID = userId;
            order.Status = "Pending";
            order.OrderType = orderType;
            order.OrderItems = null;
            order.TotalAmount = 0;

            var orderItems = new List<OrderItemEntity>();

            if (request.OrderItems.Any())
            {
                foreach (var item in request.OrderItems)
                {
                    var validatorOrderItem = await _createOrderItemValidator.ValidateAsync(item);
                    if (!validatorOrderItem.IsValid)
                    {
                        var errors = validationResult.Errors.Select(e => new
                        {
                            Field = e.PropertyName,
                            Message = e.ErrorMessage
                        });
                        throw new ApiException("Giỏ hàng không hợp lệ.", 400, errors);
                    }

                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    var lastVersion = await _productVersionRepository.GetLastProductVersionByProductIdAsync(item.ProductId);

                    if (lastVersion == null)
                    {
                        throw new ApiException("Sản phẩm không khả dụng.", 400);
                    }

                    if (product == null)
                    {
                        throw new ApiException("Không tim thấy sản phẩm.", 404);
                    }

                    if (product.Quantity < item.Quantity)
                    {
                        throw new ApiException($"Không đủ sản phẩm {product.Name}.Số lượng hiện tại: {product.Quantity}", 400);
                    }

                    var newOrderItem = _mapper.Map<OrderItemEntity>(item);
                    newOrderItem.Price = lastVersion.Price;
                    newOrderItem.Note = item.Note;

                    order.TotalAmount += newOrderItem.Price * newOrderItem.Quantity;

                    orderItems.Add(newOrderItem);

                    product.Quantity -= item.Quantity;
                    _productRepository.UpdateWithoutSaving(product);
                }

                order.OrderItems = orderItems;
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var newOrder = await _orderRepository.AddAsync(order);
                var newInvoice = new InvoiceEntity
                {
                    OrderID = newOrder.ID,
                    TotalAmount = order.TotalAmount,
                    Discount = request.Discount,
                };
                var invoice = await _invoiceRepository.AddAsync(newInvoice);

                // add payment to db
                var newPayment = _mapper.Map<PaymentEntity>(request.PaymentInfo);
                newPayment.InvoiceId = invoice.ID;
                var payment = await _paymentRepository.AddWithoutSavingAsync(newPayment);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Order created successfully");

                var listOI = await _orderItemRepository.GetByOrderId(newOrder.ID);
                var result = _mapper.Map<OrderResponse>(newOrder);
                result.OrderItems = _mapper.Map<ICollection<OrderItemResponse>>(listOI);

                //if is roleId = 2 (customer) and orderType = online then create payment with methods include(vnpay, paypal, momo)
                var paymentUrl = string.Empty;
                if (userRole == 2 && orderType == "online")
                {
                    paymentUrl = GetPaymentUrl(request.PaymentInfo.PaymentMethod, newOrder.ID, order.TotalAmount / 2);
                }
                result.PaymentUrl = paymentUrl;

                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new ApiException("Lỗi trong quá trình đặt hàng và thanh toán.", 500);
            }
        }

        private string GetPaymentUrl(string paymentMethod, int orderId, decimal totalAmount)
        {
            if (paymentMethod.ToLower() == "vnpay")
            {
                var vnPayRequest = new VNPayRequest
                {
                    OrderId = orderId,
                    Amount = totalAmount,
                    OrderInfo = $"Thanh toán đơn hàng {orderId}",
                };
                return _vnpayService.CreatePaymentUrl(vnPayRequest, base.GetIpAddress());
            }
            else
            {
                throw new ApiException($"Unsupported payment method: {paymentMethod}", 400);
            }
        }

        // if the order is online, send notifications to the customer and the restaurant
        //private async Task SendOrderNotifications(OrderEntity order, string orderType)
        //{
        //    // Different notification strategies based on order type
        //    if (orderType == "online")
        //    {
        //        // Send confirmation email to customer
        //        await _notificationService.SendOrderConfirmationEmailAsync(order);

        //        // Notify restaurant about online order
        //        await _notificationService.NotifyRestaurantAboutOrderAsync(order);
        //    }
        //    else // in_restaurant
        //    {
        //        // Maybe notify kitchen staff
        //        await _notificationService.NotifyKitchenAboutOrderAsync(order);
        //    }
        //}

        public async Task DeleteOrderAsync(int id)
        {
            var checkOrder = await _orderRepository.GetByIdAsync(id);

            if (checkOrder == null)
            {
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
                throw new ApiException("Order not found", 404);
            }

            var result = _mapper.Map<ICollection<OrderResponse>>(orders);

            _logger.LogInformation("Order listed successfully");

            return result;
        }

        public async Task<OrderResponse> GetOrderById(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null || (order.UserID != this.GetCurrentUserId()))
            {
                throw new ApiException("Không tìm thấy đơn hàng", 404);
            }

            var listOI = await _orderItemRepository.GetByOrderId(order.ID);

            var result = _mapper.Map<OrderResponse>(order);
            result.OrderItems = _mapper.Map<ICollection<OrderItemResponse>>(listOI);

            _logger.LogInformation("Order listed successfully");

            return result;
        }

        public async Task<ICollection<OrderResponse>> GetOrderRangeDate(DateTime startDate, DateTime endDate)
        {
            var orders = await _orderRepository.GetByDateRangeAsync(startDate, endDate);

            if (orders == null)
            {
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
                throw new ApiException("Order not found", 404);
            }

            foreach (var item in request.OrderItems)
            {
                var validatorOrderItem = await _updateOrderItemValidator.ValidateAsync(item);
                if (!validatorOrderItem.IsValid)
                {
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
            result.OrderItems = _mapper.Map<ICollection<OrderItemResponse>>(listOI);

            return result;
        }

        public async Task<ICollection<OrderResponse>> GetPendingOrdersAsync()
        {
            var orders = await _orderRepository.GetPendingOrdersAsync();

            var result = _mapper.Map<ICollection<OrderResponse>>(orders);

            _logger.LogInformation("Order listed successfully");

            return result;
        }

        public async Task<ICollection<OrderResponse>> GetAllOrdersAsync(int page, int pageSize)
        {
            var orders = await _orderRepository.GetAllOrdersAsync(page, pageSize);

            var totalRecords = await _orderRepository.CountAllAsync();
            _httpContextAccessor.HttpContext.SetPaginationInfo(totalRecords, page, pageSize, orders.Count());

            var result = _mapper.Map<ICollection<OrderResponse>>(orders);

            _logger.LogInformation("Order listed successfully");

            return result;
        }
    }
}
