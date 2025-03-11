using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.DTOs.Requests.Auth;
using FoodFlowSystem.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace FoodFlowSystem.Repositories.Auth
{
    public class AuthRepository : BaseRepository<UserEntity>, IAuthRepository
    {
        public AuthRepository(MssqlDbContext dbContext) : base(dbContext)
        {

        }

        public Task<IEnumerable<UserEntity>> GetByRoleNameAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<UserEntity> CheckUser(string email, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email && x.HashPassword == password);
            return user;
        }

        public Task<UserEntity> IsExistUserAsync(string input)
        {
            if (input.Contains("@"))
            {
                return _dbContext.Users.FirstOrDefaultAsync(x => x.Email == input);
            }
            else
            {
                return _dbContext.Users.FirstOrDefaultAsync(x => x.Phone == input);
            }
        }
    }
}