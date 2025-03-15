using FoodFlowSystem.Data.DbContexts;
using FoodFlowSystem.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace FoodFlowSystem.Repositories.User
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(MssqlDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<IEnumerable<UserEntity>> GetByNameAsync(string name)
        {
            return await _dbContext.Users.Where(x => x.LastName.Contains(name)).ToListAsync();
        }

        public Task<UserEntity> IsExistUserAsync(string input)
        {
            if (int.TryParse(input, out int id))
            {
                return _dbContext.Users.FirstOrDefaultAsync(x => x.ID == id);
            }

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
