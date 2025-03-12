using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Repositories.Auth
{
    public interface IAuthRepository : IBaseRepository<UserEntity>
    {
        Task<UserEntity> CheckUser(string email, string password);
    }
}