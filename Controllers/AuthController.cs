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
            var  response = await _authService.LoginWithGoogleAsync(request);
            
            return Ok(response);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await _authService.RegisterAsync(request);
            return Ok();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            await _authService.LoginAsync(request);
            
            return Ok();
        }

        [HttpGet("verify-token")]
        [Authorize]
        public async Task<IActionResult> VerifyToken()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "user_id").Value);
            var result = await _userService.GetUserByIdAsync(userId);
            return Ok(result);
        }
    }
}
