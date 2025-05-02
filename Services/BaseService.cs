using FoodFlowSystem.DTOs;
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

        protected string GetIpAddress()
        {
            string ipAddress;
            try
            {
                ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

                if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1")
                {
                    ipAddress = "127.0.0.1";
                }
            }
            catch (Exception)
            {
                ipAddress = "127.0.0.1";
            }

            return ipAddress;
        }

        protected int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                throw new ApiException("Người dùng chưa đăng nhập", 401);
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new ApiException("Thông tin người dùng không hợp lệ", 401);
            }

            return userId;
        }

        protected int GetCurrentUserRole()
        {
            var roleIdClaim = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "roleId");

            if (roleIdClaim == null || string.IsNullOrEmpty(roleIdClaim.Value))
            {
                throw new ApiException("Người dùng chưa đăng nhập hoặc không có quyền", 401);
            }

            if (!int.TryParse(roleIdClaim.Value, out int roleId))
            {
                throw new ApiException("Thông tin quyền người dùng không hợp lệ", 401);
            }

            return roleId;
        }
    }
}
