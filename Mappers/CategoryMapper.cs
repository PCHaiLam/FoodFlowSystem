using AutoMapper;
using FoodFlowSystem.DTOs.Requests.Category;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Entities.Category;

namespace FoodFlowSystem.Mappers
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            //request to entity
            CreateMap<CreateCategoryRequest, CategoryEntity>();
            CreateMap<UpdateCategoryRequest, CategoryEntity>();

            //entity to response
            CreateMap<CategoryEntity, CategoryResponse>();

        }
    }
}
