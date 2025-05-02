using AutoMapper;
using FluentValidation;
using FoodFlowSystem.DTOs;
using FoodFlowSystem.DTOs.Requests.Auth;
using FoodFlowSystem.Entities.OAuth;
using FoodFlowSystem.Entities.User;
using FoodFlowSystem.Helpers;
using FoodFlowSystem.Repositories.Auth;
using FoodFlowSystem.Repositories.EmailTemplates;
using FoodFlowSystem.Repositories.OAuth;
using FoodFlowSystem.Repositories.User;
using FoodFlowSystem.Services.SendMail;
using Google.Apis.Auth;
using System.Security.Claims;

namespace FoodFlowSystem.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOAuthRepository _oauthRepository;
        private readonly IEmailTemplatesRepository _emailTemplatesRepository;
        private readonly ISendMailService _sendMailService;
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
            IEmailTemplatesRepository emailTemplatesRepository,
            ISendMailService sendMailService,
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
            _emailTemplatesRepository = emailTemplatesRepository;
            _sendMailService = sendMailService;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _jwtHelper = jwtHelper;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        public async Task LoginWithGoogleAsync(GoogleLoginRequest request)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
                if (payload == null)
                {
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

                    var emailTemplate = await _emailTemplatesRepository.GetTemplateByNameAsync("WelcomeNewUser");
                    if (emailTemplate == null)
                    {
                        throw new ApiException("Email template not found", 404);
                    }

                    var emailBody = emailTemplate.Body
                        .Replace("{firstName}", newUser.FirstName)
                        .Replace("{fullName}", newUser.LastName + " " + newUser.FirstName)
                        .Replace("{web}", "http://localhost:5173/");

                    await _sendMailService.SendMailAsync(newUser.Email, emailTemplate.Subject, emailBody);

                    _logger.LogInformation("Register user with google account success");
                }

                user = await _userRepository.IsExistUserEmailAsync(payload.Email);

                var claims = new[]
                {
                new Claim("userId", user.ID.ToString()),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName ?? ""),
                new Claim("phone", user.Phone ?? ""),
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
            catch (InvalidJwtException ex)
            {
                throw new ApiException("Token Google không hợp lệ hoặc đã hết hạn", 401);
            }
            catch (Exception ex)
            {
                throw new ApiException("Không thể xác thực với Google. Vui lòng thử lại sau", 500);
            }
        }

        public async Task LoginAsync(LoginRequest request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
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
                throw new ApiException("User doesn't exist", 400);
            }
            else
            {
                var passWordHashed = HashPassword.Hash(request.Password);
                if (user.HashPassword != passWordHashed)
                {
                    throw new ApiException("Invalid password", 400);
                }
            }

            var claims = new[]
            {
                new Claim("userId", user.ID.ToString()),
                new Claim("firstName", user.FirstName ?? string.Empty),
                new Claim("lastName", user.LastName ?? string.Empty),
                new Claim("email", user.Email ?? string.Empty),
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
