using AutoMapper;
using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Auth;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Entities.OAuth;
using FoodFlowSystem.Entities.User;
using FoodFlowSystem.Helpers;
using FoodFlowSystem.Middlewares.Exceptions;
using FoodFlowSystem.Repositories.Auth;
using FoodFlowSystem.Repositories.OAuth;
using FoodFlowSystem.Repositories.User;
using Google.Apis.Auth;
using System.Security.Claims;

namespace FoodFlowSystem.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOAuthRepository _oauthRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtHelper _jwtHelper;
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IValidator<RegisterRequest> _registerValidator;

        public AuthService(
            IAuthRepository authRepository, 
            IUserRepository userRepository,
            IOAuthRepository oauthRepository,
            ILogger<AuthService> logger, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor, 
            JwtHelper jwtHelper,
            IValidator<LoginRequest> loginValidator,
            IValidator<RegisterRequest> registerValidator)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _oauthRepository = oauthRepository;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _jwtHelper = jwtHelper;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        public async Task LoginWithGoogleAsync(GoogleLoginRequest request)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
            if (payload == null)
            {
                _logger.LogError("Invalid google payload");
                throw new ApiException("Invalid google payload", 400);
            }
            
            var user = await _userRepository.IsExistUserEmailAsync(payload.Email);
            if (user == null)
            {
                var newUser = new UserEntity
                {
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    Email = payload.Email,
                    PhotoUrl = payload.Picture,
                    RoleID = 2,
                };

                await _authRepository.AddAsync(newUser);

                var newOauth = new OAuthEntity
                {
                    Provider = "GOOGLE",
                    ProviderUserId = payload.Subject,
                    UserId = newUser.ID,
                    Email = payload.Email,
                    LastLoginAt = DateTime.UtcNow,
                };

                await _oauthRepository.AddAsync(newOauth);
                _logger.LogInformation("Register user with google account success");
            }

            user = await _userRepository.IsExistUserEmailAsync(payload.Email);

            var claims = new[]
            {
                new Claim("userId", user.ID.ToString()),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName ?? ""),
                new Claim("phoneNumber", user.Phone ?? ""),
                new Claim("photoUrl", user.PhotoUrl ?? ""),
                new Claim("email", user.Email),
                new Claim("roleId", user.RoleID.ToString()),
            };

            var token = _jwtHelper.GenerateToken(claims);
            var refreshToken = _jwtHelper.CreateRefreshToken();

            var responseHeaders = _httpContextAccessor.HttpContext.Response.Headers;
            responseHeaders.Append("auth_token", $"Bearer {token}");
            responseHeaders.Append("refresh_token", refreshToken);

            _logger.LogInformation("Login with google success");
        }   
        
        public async Task LoginAsync(LoginRequest request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogError("Validation error: {0}", validationResult.Errors);
                var errors = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                });
                throw new ApiException("Invalid email or password.", 400, errors);
            }

            var user = await _authRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogError("User doesn't exist");
                throw new ApiException("User doesn't exist", 400);
            } 
            else
            {
                var passWordHashed = HashPassword.Hash(request.Password);
                if (user.HashPassword != passWordHashed)
                {
                    _logger.LogError("Invalid password");
                    throw new ApiException("Invalid password", 400);
                }
            }

            var claims = new[]
            {
                new Claim("userId", user.ID.ToString()),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
                new Claim("email", user.Email),
                new Claim("phoneNumber", user.Phone),
                new Claim("roleId", user.RoleID.ToString()),
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
                var errors = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                });
                throw new ApiException("Invalid input", 400, errors);
            }

            var emailExist = await _userRepository.IsExistUserEmailAsync(request.Email);
            if (emailExist != null)
            {
                _logger.LogError("Email already exists");
                throw new ApiException("Email already exists", 400);
            }

            var newUser = _mapper.Map<UserEntity>(request);

            newUser.HashPassword = HashPassword.Hash(request.Password);
            newUser.RoleID = 2;

            await _authRepository.AddAsync(newUser);
            _logger.LogInformation("Register success");
        }
    }
}
