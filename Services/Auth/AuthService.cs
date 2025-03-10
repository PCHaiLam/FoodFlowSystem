using AutoMapper;
using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Auth;
using FoodFlowSystem.Entities.User;
using FoodFlowSystem.Helpers;
using FoodFlowSystem.Middlewares.Exceptions;
using FoodFlowSystem.Repositories.Auth;
using System.Security.Claims;

namespace FoodFlowSystem.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtHelper _jwtHelper;
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IValidator<RegisterRequest> _registerValidator;

        public AuthService(
            IAuthRepository authRepository, 
            ILogger<AuthService> logger, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor, 
            JwtHelper jwtHelper,
            IValidator<LoginRequest> loginValidator,
            IValidator<RegisterRequest> registerValidator)
        {
            _authRepository = authRepository;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _jwtHelper = jwtHelper;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        public async Task LoginAsync(LoginRequest request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogError("Validation error: {0}", validationResult.Errors);
                throw new ApiException("Invalid email or password.", 400);
            }

            var user = await _authRepository.CheckUser(request.Email, request.Password);
            if (user == null)
            {
                _logger.LogError("Email or password wrong!");
                throw new ApiException("Email or password wrong!", 400);
            }

            var claims = new[]
            {
                new Claim("userId", user.ID.ToString()),
                new Claim("name", user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("phone", user.Phone),
                new Claim(ClaimTypes.Role, user.RoleID.ToString()),
            };

            var token = _jwtHelper.GenerateToken(claims);
            var refreshToken = _jwtHelper.CreateRefreshToken();

            var responseHeaders = _httpContextAccessor.HttpContext.Response.Headers;
            responseHeaders.Append("auth_token", $"Bearer {token}");
            responseHeaders.Append("refresh_token", refreshToken);

            _logger.LogInformation("Login success");
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            var validationResult = _registerValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                _logger.LogError("Validation error: {0}", validationResult.Errors);
                throw new ApiException("Invalid input", 400);
            }

            var newUser = _mapper.Map<UserEntity>(request);
            newUser.HashPassword = HashPassword.Hash(request.Password);

            await _authRepository.AddAsync(newUser);
            _logger.LogInformation("Register success");
        }
    }
}
