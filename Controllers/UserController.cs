using FoodFlowSystem.DTOs.Requests.User;
using FoodFlowSystem.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodFlowSystem.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var result = await _userService.GetUsersAsync();
            return Ok(result);
        }

        [HttpGet("name")]
        [Authorize]
        public async Task<IActionResult> GetUserByNameAsync([FromQuery] string name)
        {
            var result = await _userService.GetUserByNameAsync(name);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest request)
        {
            var result = await _userService.UpdateUserAsync(request);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> DeleteUser(int input)
        {
            await _userService.DeleteUserAsync(input);
            return Ok();
        }
    }
}
