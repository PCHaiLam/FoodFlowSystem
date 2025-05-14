using FoodFlowSystem.Entities.Token;

namespace FoodFlowSystem.Repositories.Token
{
    public interface ITokenRepository : IBaseRepository<TokenEntity>
    {
        Task<TokenEntity> GetByRefreshTokenAsync(string refreshToken);
        Task<List<TokenEntity>> GetTokensByUserIdAsync(int userId);
        Task RevokeTokenAsync(TokenEntity token);
        Task RevokeAllUserTokensAsync(int userId);
    }
}