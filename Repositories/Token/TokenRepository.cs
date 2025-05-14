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

        public async Task<TokenEntity> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _dbContext.Set<TokenEntity>()
                .FirstOrDefaultAsync(t => t.RefreshToken == refreshToken && !t.IsRevoked);
        }

        public async Task<List<TokenEntity>> GetTokensByUserIdAsync(int userId)
        {
            return await _dbContext.Set<TokenEntity>()
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task RevokeTokenAsync(TokenEntity token)
        {
            token.IsRevoked = true;
            token.UpdatedAt = DateTime.UtcNow;
            await UpdateAsync(token);
        }

        public async Task RevokeAllUserTokensAsync(int userId)
        {
            var tokens = await GetTokensByUserIdAsync(userId);
            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.UpdatedAt = DateTime.UtcNow;
                UpdateWithoutSaving(token);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}