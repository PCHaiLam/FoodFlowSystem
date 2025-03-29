using AutoMapper;
using FluentValidation;
using FoodFlowSystem.DTOs.Requests.User;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Middlewares.Exceptions;
using FoodFlowSystem.Repositories.User;

namespace FoodFlowSystem.Services.User
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IValidator<UpdateUserRequest> _updateUserValidator;
        private readonly IValidator<CreateUserRequest> _createUserValidator;

        public UserService(
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            ILogger<UserService> logger,
            IValidator<UpdateUserRequest> updateUserValidator,
            IValidator<CreateUserRequest> createUserValidator
            ) : base(httpContextAccessor)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _updateUserValidator = updateUserValidator;
            _createUserValidator = createUserValidator;
        }

        public Task<UserResponse> CreateUserAsync(CreateUserRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUserAsync(int id)
        {
            var checkUser = _userRepository.GetByIdAsync(id);

            if (checkUser == null)
            {
                _logger.LogError("User not found");
                throw new ApiException("User not found", 404);
            }

            await _userRepository.DeleteAsync(id);

            _logger.LogInformation("User deleted successfully");
        }

        public async Task<UserResponse> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogError("User not found");
                throw new ApiException("User not found", 404);
            }

            var result = _mapper.Map<UserResponse>(user);
            _logger.LogInformation("User listed successfully");

            return result;
        }

        public async Task<IEnumerable<UserResponse>> GetUserByNameAsync(string name)
        {
            var user = await _userRepository.GetByNameAsync(name);
            if (user == null)
            {
                _logger.LogError("User not found");
                throw new ApiException("User not found", 404);
            }

            var result = _mapper.Map<IEnumerable<UserResponse>>(user);
            _logger.LogInformation($"Searched successfully");
            return result;
        }

        public async Task<IEnumerable<UserResponse>> GetUsersAsync()
        {
            var list = await _userRepository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<UserResponse>>(list);

            _logger.LogInformation("Users listed successfully");

            return result;
        }

        public async Task<UserResponse> UpdateUserAsync(UpdateUserRequest request)
        {
            var validationResult = await _updateUserValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogError("Invalid Input");
                var errors = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                });
                throw new ApiException("Invalid Input", 400, errors);
            }

            var userId = this.GetCurrentUserId();

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("User not found");
                throw new ApiException("User not found", 404);
            }

            var userDto = await _userRepository.UpdateAsync(user);
            var result = _mapper.Map<UserResponse>(userDto);

            _logger.LogInformation("User updated successfully");

            return result;
        }
    }
}
