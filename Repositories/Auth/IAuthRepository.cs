using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Repositories.Auth
{
    public interface IAuthRepository : IBaseRepository<UserEntity>
    {
        Task<UserEntity> GetUserByEmailAsync(string email);
    }
}