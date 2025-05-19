using FoodFlowSystem.Entities.Token;

namespace FoodFlowSystem.Repositories.Token
{
    public interface ITokenRepository : IBaseRepository<TokenEntity>
    {
        Task<TokenEntity> GetByRefreshTokenAsync(string refreshToken);
        Task DeleteByRefreshTokenAsync(string token);
    }
}