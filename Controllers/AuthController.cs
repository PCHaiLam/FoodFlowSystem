using FoodFlowSystem.DTOs.Requests.Auth;
using FoodFlowSystem.Entities.User;
using FoodFlowSystem.Services.Auth;
using FoodFlowSystem.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodFlowSystem.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthController(
            IAuthService authService,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _authService = authService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("google-login")]
        [AllowAnonymous]
        public async Task<IActionResult> Google([FromBody] GoogleLoginRequest request)
        {
            await _authService.LoginWithGoogleAsync(request);
            
            return Ok();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await _authService.RegisterAsync(request);
            return Ok();
        }

        [HttpPost("admin-login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            await _authService.LoginAsync(request);
            
            return Ok();
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.RefreshTokenAsync(request);
            return Ok(result);
        }
    }
}
