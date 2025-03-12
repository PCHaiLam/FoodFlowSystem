using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Repositories.User
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        Task<UserEntity> IsExistUserAsync(string input);
        Task<IEnumerable<UserEntity>> GetByNameAsync(string name);
    }
}
