using FoodFlowSystem.DTOs.Requests.Auth;
using FoodFlowSystem.Entities.User;
using FoodFlowSystem.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodFlowSystem.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IHttpClientFactory _clientFactory;
        
        public AuthController(
            IAuthService authService,
            IHttpClientFactory httpClientFactory)
        {
            _authService = authService;
            _clientFactory = httpClientFactory;
        }

        [HttpPost("google")]
        [AllowAnonymous]
        public async Task<IActionResult> Google([FromBody] GoogleTokenRequest request)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={request.Token}");
            var token = await response.Content.ReadAsStringAsync();
            var user = await client.GetFromJsonAsync<UserEntity>("https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + token);
            
            return Ok(user);
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
    }
}
