using FoodFlowSystem.DTOs.Requests.Auth;
using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Services.Auth
{
    public interface IAuthService
    {
        Task LoginAsync(LoginRequest request);
        Task RegisterAsync(RegisterRequest request);
    }
}
