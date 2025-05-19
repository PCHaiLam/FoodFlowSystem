using AutoMapper;
using Azure.Core;
using FluentValidation;
using FoodFlowSystem.DTOs;
using FoodFlowSystem.DTOs.Requests.Auth;
using FoodFlowSystem.Entities.OAuth;
using FoodFlowSystem.Entities.Token;
using FoodFlowSystem.Entities.User;
using FoodFlowSystem.Helpers;
using FoodFlowSystem.Repositories.Auth;
using FoodFlowSystem.Repositories.EmailTemplates;
using FoodFlowSystem.Repositories.OAuth;
using FoodFlowSystem.Repositories.Token;
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
        private readonly ITokenRepository _tokenRepository;

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
            IValidator<RegisterRequest> registerValidator,
            ITokenRepository tokenRepository)
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
            _tokenRepository = tokenRepository;
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

                var claims = new[]
                {
                    new Claim("userId", user.ID.ToString()),
                    new Claim("firstName", user.FirstName),
                    new Claim("lastName", user.LastName ?? ""),
                    new Claim("phone", user.Phone ?? ""),
                    new Claim("photoUrl", user.PhotoUrl ?? ""),
                    new Claim("email", user.Email),
                    new Claim(ClaimTypes.Role, user.RoleID.ToString()),
                };

                var accessToken = _jwtHelper.GenerateToken(claims);
                var refreshToken = _jwtHelper.CreateRefreshToken();
                var expiresAt = _jwtHelper.GetRefreshTokenExpiryTime();

                var tokenEntity = new TokenEntity
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = expiresAt,
                    UserID = user.ID
                };

                await _tokenRepository.AddAsync(tokenEntity);

                var responseHeaders = _httpContextAccessor.HttpContext.Response.Headers;
                responseHeaders.Append("auth_token", $"Bearer {accessToken}");
                responseHeaders.Append("refresh_token", refreshToken);

                _logger.LogInformation("Login with google success");
            }
            catch (InvalidJwtException ex)
            {
                _logger.LogError(ex, "Invalid JWT token");
                throw new ApiException("Token Google không hợp lệ hoặc đã hết hạn", 401);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Google login");
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
                new Claim(ClaimTypes.Role, user.RoleID.ToString()),
            };

            var accessToken = _jwtHelper.GenerateToken(claims);
            var refreshToken = _jwtHelper.CreateRefreshToken();
            var expiresAt = _jwtHelper.GetRefreshTokenExpiryTime();

            var tokenEntity = new TokenEntity
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = expiresAt,
                UserID = user.ID,
            };

            await _tokenRepository.AddAsync(tokenEntity);

            var responseHeaders = _httpContextAccessor.HttpContext.Response.Headers;
            responseHeaders.Append("auth_token", $"Bearer {accessToken}");
            responseHeaders.Append("refresh_token", refreshToken);

            _logger.LogInformation("Login success");
        }

        public async Task RefreshTokenAsync(RefreshTokenRequest request)
        {
            var storedToken = await _tokenRepository.GetByRefreshTokenAsync(request.RefreshToken);
            if (storedToken == null)
            {
                throw new ApiException("Refresh token không hợp lệ", 401);
            }

            var user = await _userRepository.GetByIdAsync(storedToken.UserID);
            if (user == null)
            {
                throw new ApiException("Không tìm thấy người dùng", 404);
            }

            var claims = new[]
            {
                new Claim("userId", user.ID.ToString()),
                new Claim("firstName", user.FirstName ?? string.Empty),
                new Claim("lastName", user.LastName ?? string.Empty),
                new Claim("email", user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, user.RoleID.ToString()),
            };

            var newAccessToken = _jwtHelper.GenerateToken(claims);
            string newRefreshToken;
            DateTime newExpiresAt;

            if (storedToken.ExpiresAt > DateTime.UtcNow)
            {
                newRefreshToken = storedToken.RefreshToken;
                storedToken.AccessToken = newAccessToken;

                await _tokenRepository.UpdateAsync(storedToken);

                _logger.LogInformation("Access token refreshed successfully");
            }
            else
            {
                newRefreshToken = _jwtHelper.CreateRefreshToken();
                newExpiresAt = _jwtHelper.GetRefreshTokenExpiryTime();

                storedToken.AccessToken = newAccessToken;
                storedToken.RefreshToken = newRefreshToken;
                storedToken.ExpiresAt = newExpiresAt;
                await _tokenRepository.UpdateAsync(storedToken);

                _logger.LogInformation("Access token and refresh token refreshed successfully");
            }

            var responseHeaders = _httpContextAccessor.HttpContext.Response.Headers;
            responseHeaders.Append("auth_token", $"Bearer {newAccessToken}");
            responseHeaders.Append("refresh_token", newRefreshToken);
        }

        public async Task LogoutAsync(RefreshTokenRequest request)
        {
            var token = await _tokenRepository.GetByRefreshTokenAsync(request.RefreshToken);
            if (token != null)
            {
                await _tokenRepository.DeleteByRefreshTokenAsync(request.RefreshToken);
                _logger.LogInformation("Logout success");
            }
            else
            {
                throw new ApiException("Token không hợp lệ", 401);
            }
        }
    }
}
