using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Repositories.Auth
{
    public interface IAuthRepository : IBaseRepository<UserEntity>
    {
        Task<IEnumerable<UserEntity>> GetByRoleNameAsync(string roleName);
        Task<UserEntity> CheckUser(string email, string password);
    }
}
