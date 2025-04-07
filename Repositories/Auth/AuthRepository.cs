using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace FoodFlowSystem.Repositories.Auth
{
    public class AuthRepository : BaseRepository<UserEntity>, IAuthRepository
    {
        public AuthRepository(MssqlDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            return user;
        }
    }
}