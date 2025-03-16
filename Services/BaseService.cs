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
            return int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "user_id").Value);
        }
    }
}
