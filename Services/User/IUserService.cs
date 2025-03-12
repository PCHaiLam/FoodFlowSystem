using FoodFlowSystem.DTOs.Requests.User;
using FoodFlowSystem.DTOs.Responses;

namespace FoodFlowSystem.Services.User
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetUsersAsync();
        Task<IEnumerable<UserResponse>> GetUserByNameAsync(string name);
        Task<UserResponse> CreateUserAsync(CreateUserRequest request);
        Task<UserResponse> UpdateUserAsync(UpdateUserRequest request);
        Task DeleteUserAsync(int id);
    }
}
