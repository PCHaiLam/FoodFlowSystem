using AutoMapper;
using FoodFlowSystem.DTOs.Requests.Auth;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Entities.User;

namespace FoodFlowSystem.Mappers
{
    public class AuthMapper : Profile
    {
        public AuthMapper()
        {
            //request to entity
            CreateMap<RegisterRequest, UserEntity>();

            //entity to response
            CreateMap<UserEntity, UserResponse>();
        }
    }
}
