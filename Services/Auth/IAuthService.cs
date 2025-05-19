using FoodFlowSystem.DTOs.Requests.Auth;
using FoodFlowSystem.DTOs.Responses;

namespace FoodFlowSystem.Services.Auth
{
    public interface IAuthService
    {
        Task LoginWithGoogleAsync(GoogleLoginRequest request);
        Task LoginAsync(LoginRequest request);
        Task RefreshTokenAsync(RefreshTokenRequest request);
        Task LogoutAsync(RefreshTokenRequest request);
    }
}
