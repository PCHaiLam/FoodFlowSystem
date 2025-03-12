using AutoMapper;
using FoodFlowSystem.DTOs.Requests.Auth;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            //request to entity
            CreateMap<RegisterRequest, UserEntity>();

            //entity to response
            CreateMap<UserEntity, UserResponse>();
        }
    }
}
