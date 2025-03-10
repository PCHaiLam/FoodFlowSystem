using FoodFlowSystem.DTOs.Requests.Auth;

namespace FoodFlowSystem.Services.Auth
{
    public interface IAuthService
    {
        Task LoginAsync(LoginRequest request);
        Task RegisterAsync(RegisterRequest request);
    }
}
