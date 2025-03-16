using AutoMapper;
using FoodFlowSystem.DTOs.Requests.User;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            //request to entity
            CreateMap<CreateUserRequest, UserEntity>();

            //entity to response
            CreateMap<UserEntity, UserResponse>();
        }
    }
}
