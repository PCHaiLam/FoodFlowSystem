using AutoMapper;
using FoodFlowSystem.DTOs.Requests.Product;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Entities.Product;

namespace FoodFlowSystem.Mappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            // request -> entity
            CreateMap<CreateProductRequest, ProductEntity>();
            CreateMap<UpdateProductRequest, ProductEntity>();

            // entity -> response
            CreateMap<ProductEntity, ProductResponse>();
        }
    }
}
