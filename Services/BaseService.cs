using System.Security.Claims;

namespace FoodFlowSystem.Services
{
    public abstract class BaseService
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        protected BaseService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected int GetCurrentUserId()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "user_id").Value);
            return userId;
        }
        protected int GetCurrentUserRole()
        {
            var roleId = int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value);
            return roleId;
        }
    }
}
