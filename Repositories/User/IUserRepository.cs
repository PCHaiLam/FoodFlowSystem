using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Repositories.User
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        Task<UserEntity> IsExistUserEmailAsync(string input);
        Task<IEnumerable<UserEntity>> GetByNameAsync(string name);
        Task<ICollection<UserEntity>> GetByArangeDateAsync(DateTime startDate, DateTime endDate);
    }
}
