using FoodFlowSystem.DTOs.Requests.Auth;

namespace FoodFlowSystem.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginRequest request);
        Task<bool> RegisterAsync(RegisterRequest request);
    }
}
