using FoodFlowSystem.DTOs.Requests.Auth;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.DTOs.Responses.Auth;

namespace FoodFlowSystem.Services.Auth
{
    public interface IAuthService
    {
        Task LoginWithGoogleAsync(GoogleLoginRequest request);
        Task LoginAsync(LoginRequest request);
        Task RegisterAsync(RegisterRequest request);
        Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
