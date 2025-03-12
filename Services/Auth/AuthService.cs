using AutoMapper;
using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Auth;
using FoodFlowSystem.Entities.User;
using FoodFlowSystem.Helpers;
using FoodFlowSystem.Middlewares.Exceptions;
using FoodFlowSystem.Repositories.Auth;
using FoodFlowSystem.Repositories.User;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Security.Claims;

namespace FoodFlowSystem.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtHelper _jwtHelper;
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IValidator<RegisterRequest> _registerValidator;

        public AuthService(
            IAuthRepository authRepository, 
            IUserRepository userRepository,
            ILogger<AuthService> logger, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor, 
            JwtHelper jwtHelper,
            IValidator<LoginRequest> loginValidator,
            IValidator<RegisterRequest> registerValidator)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _jwtHelper = jwtHelper;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        //public async Task AuthenticateWithGoogleAsync(string accessToken, string idToken)
        //{
        //    // Validate the ID token and retrieve the payload
        //    var payload = await ValidateGoogleTokenAsync(idToken);

        //    // Fetch additional user profile information using the access token
        //    var userProfile = await GetGoogleUserProfileAsync(accessToken);

        //    // Check if the user already exists in the database
        //    var userExists = await _authRepository.IsExistUserAsync(payload.Email);

        //    // If user doesn't exist, create a new user
        //    if (userExists == null)
        //    {
        //        await CreateNewUserAsync(userProfile);

        //    }

        //    // Prepare the sign-in DTO for login
        //    var signInDto = new SignInDto(
        //        Email: userProfile.Email,
        //        Password: string.Empty,  // No password for external logins
        //        SignUpMethod: SignUpMethod.External
        //    );

        //    // Use the login service to issue a token
        //    var token = await connectService.LoginAsync(signInDto);

        //    return token;
        //}
        //private async Task<GoogleUserProfile> GetGoogleUserProfileAsync(string accessToken)
        //{
        //    using var httpClient = new HttpClient();

        //    // Send GET request to Google UserInfo API
        //    var response = await httpClient.GetAsync($"{_googleUserInfoUrl}?access_token={accessToken}");
        //    response.EnsureSuccessStatusCode();

        //    // Parse the JSON response
        //    var content = await response.Content.ReadAsStringAsync();
        //    var userProfile = JsonConvert.DeserializeObject<GoogleUserProfile>(content);

        //    return userProfile!;
        //}

        //private static async Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string idToken)
        //{
        //    var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

        //    if (payload == null || string.IsNullOrEmpty(payload.Email))
        //        throw new UnauthorizedAccessException("Invalid Google Token");

        //    return payload;
        //}

        //private async Task CreateNewUserAsync(GoogleUserProfile userProfile)
        //{
        //    var newUser = new UserEntity
        //    {
        //        FullName = userProfile.Name,
        //        Email = userProfile.Email,
        //    };

        //    // Add the new user to the repository and save
        //    await identityUserRepository.AddAsync(newUser);
        //    await identityUserRepository.SaveChangesAsync();
        //}

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

            var emailExist = await _userRepository.IsExistUserAsync(request.Email);
            if (emailExist != null)
            {
                _logger.LogError("Email already exists");
                throw new ApiException("Email already exists", 400);
            }

            var phoneExist = await _userRepository.IsExistUserAsync(request.Phone);
            if (phoneExist != null)
            {
                _logger.LogError("Phone already exists");
                throw new ApiException("Phone already exists", 400);
            }

            var newUser = _mapper.Map<UserEntity>(request);

            newUser.HashPassword = HashPassword.Hash(request.Password);
            newUser.RoleID = 2;

            await _authRepository.AddAsync(newUser);
            _logger.LogInformation("Register success");
        }
    }
}
