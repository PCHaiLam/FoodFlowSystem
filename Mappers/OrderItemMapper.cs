using AutoMapper;
using FoodFlowSystem.DTOs.Requests.OrderItem;
using FoodFlowSystem.DTOs.Responses.Feedbacks;
using FoodFlowSystem.Entities.OrderItem;

namespace FoodFlowSystem.Mappers
{
    public class OrderItemMapper : Profile
    {
        public OrderItemMapper()
        {
            // request -> entity
            CreateMap<CreateOrderItemRequest, OrderItemEntity>();
            CreateMap<UpdateOrderItemRequest, OrderItemEntity>();

            CreateMap<UpdateOrderItemRequest, CreateOrderItemRequest>();
            CreateMap<CreateOrderItemRequest, UpdateOrderItemRequest>();

            // entity -> response
            CreateMap<OrderItemEntity, OrderItemResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name));

        }
    }
}
