using AutoMapper;
using FluentValidation;
using FoodFlowSystem.DTOs;
using FoodFlowSystem.DTOs.Requests.Feedback;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Entities.Feedback;
using FoodFlowSystem.Repositories.Feedback;

namespace FoodFlowSystem.Services.Feedback
{
    public class FeedbackService : BaseService, IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FeedbackService> _logger;
        private readonly IValidator<CreateFeedbackRequest> _createFeedbackValidator;
        private readonly IValidator<UpdateFeedbackRequest> _updateFeedbackValidator;

        public FeedbackService(
            IHttpContextAccessor httpContextAccessor,
            IFeedbackRepository feedbackRepository,
            IMapper mapper,
            ILogger<FeedbackService> logger,
            IValidator<CreateFeedbackRequest> createFeedbackValidator
            ) : base( httpContextAccessor )
        {
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
            _logger = logger;
            _createFeedbackValidator = createFeedbackValidator;
        }

        public async Task<FeedbackResponse> CreateFeedbackAsync(CreateFeedbackRequest request)
        {
            var validationResult = await _createFeedbackValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                });
                throw new ApiException("Invalid request", 400, errors);
            }

            var currentUserId = this.GetCurrentUserId();

            var feedback = _mapper.Map<FeedbackEntity>(request);
            feedback.UserID = currentUserId;

            var createdFeedback = await _feedbackRepository.AddAsync(feedback);

            var result = _mapper.Map<FeedbackResponse>(createdFeedback);
            return result;
        }

        public async Task DeleteFeedbackAsync(int id)
        {
            var checkFeedback = await _feedbackRepository.GetByIdAsync(id);

            if (checkFeedback == null)
            {
                throw new ApiException("Feedback not found", 404);
            }

            await _feedbackRepository.DeleteAsync(id);

            _logger.LogInformation("Feedback deleted successfully");
        }

        public async Task<FeedbackResponse> GetFeedbackAsync(int id)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            if (feedback == null)
            {
                throw new ApiException("Feedback not found", 404);
            }

            var result = _mapper.Map<FeedbackResponse>(feedback);
            _logger.LogInformation($"Searched successfully");
            return result;
        }

        public async Task<ICollection<FeedbackResponse>> GetAllFeedbacksAsync()
        {
            var list = await _feedbackRepository.GetAllAsync();
            var result = _mapper.Map<ICollection<FeedbackResponse>>(list);

            _logger.LogInformation("Feedbacks listed successfully");

            return result;
        }

        public async Task<ICollection<FeedbackResponse>> GetFeedbacksByProductIdAsync(int id)
        {
            var list = await _feedbackRepository.GetByProductIdAsync(id);
            var result = _mapper.Map<ICollection<FeedbackResponse>>(list);

            _logger.LogInformation("Feedbacks listed successfully");

            return result;
        }

        public async Task<ICollection<FeedbackResponse>> GetFeedbacksByUserIdAsync(int id)
        {
            var list = await _feedbackRepository.GetByUserIdAsync(id);
            var result = _mapper.Map<ICollection<FeedbackResponse>>(list);

            _logger.LogInformation("Feedbacks listed successfully");

            return result;
        }

        public async Task<FeedbackResponse> UpdateFeedbackAsync(UpdateFeedbackRequest request)
        {
            var currentUserId = this.GetCurrentUserId();
            if (currentUserId != request.UserId)
            {
                throw new ApiException("Unauthorized this feedback", 401);
            }

            var validationResult = await _updateFeedbackValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                });
                throw new ApiException("Invalid Input", 400, errors);
            }

            var feedback = await _feedbackRepository.GetByIdAsync(request.Id);
            if (feedback == null)
            {
                throw new ApiException("Feedback not found", 404);
            }

            var updatedFeedback = _mapper.Map(request, feedback);
            await _feedbackRepository.UpdateAsync(updatedFeedback);

            var result = _mapper.Map<FeedbackResponse>(updatedFeedback);
            return result;
        }
    }
}
