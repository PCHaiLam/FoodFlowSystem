using FoodFlowSystem.DTOs.Requests.Auth;
using FoodFlowSystem.DTOs.Responses;

namespace FoodFlowSystem.Services.Auth
{
    public interface IAuthService
    {
        Task<UserResponse> LoginWithGoogleAsync(GoogleLoginRequest request);
        Task LoginAsync(LoginRequest request);
        Task RegisterAsync(RegisterRequest request);
    }
}
