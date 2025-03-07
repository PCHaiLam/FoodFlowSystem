using AutoMapper;
using FoodFlowSystem.DTOs.Requests.Auth;
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

        public AuthService(IAuthRepository authRepository, ILogger<AuthService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, JwtHelper jwtHelper)
        {
            _authRepository = authRepository;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _jwtHelper = jwtHelper;
        }

        public async Task<bool> LoginAsync(LoginRequest request)
        {
            var user = await _authRepository.CheckUser(request.Email, request.Password);
            if (user == null)
            {
                _logger.LogError("Email or password wrong!");
                throw new NotFoundException("Email or password wrong!");
            }

            var claims = new[]
            {
                new Claim("id", user.ID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.RoleID.ToString()),
            };

            var token = _jwtHelper.GenerateToken(claims);
            var refreshToken = _jwtHelper.CreateRefreshToken();

            var responseHeaders = _httpContextAccessor.HttpContext.Response.Headers;
            responseHeaders.Append("auth_token", $"Bearer {token}");
            responseHeaders.Append("refresh_token", refreshToken);

            _logger.LogInformation("Login success");
            return true;
        }

        public Task<bool> RegisterAsync(RegisterRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
