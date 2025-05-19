using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities.Token;
using Microsoft.EntityFrameworkCore;

namespace FoodFlowSystem.Repositories.Token
{
    public class TokenRepository : BaseRepository<TokenEntity>, ITokenRepository
    {
        public TokenRepository(MssqlDbContext dbContext) : base(dbContext)
        {
        }

        public async Task DeleteByRefreshTokenAsync(string token)
        {
            var tokenEntity = await GetByRefreshTokenAsync(token);
            if (tokenEntity != null)
            {
                _dbContext.Set<TokenEntity>().Remove(tokenEntity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<TokenEntity> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _dbContext.Set<TokenEntity>()
                .FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);
        }
    }
}