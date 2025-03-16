using AutoMapper;
using FoodFlowSystem.DTOs.Requests.Order;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Entities.Order;

namespace FoodFlowSystem.Mappers
{
    public class OrderMapper : Profile
    {
        public OrderMapper()
        {
            // request -> entity
            CreateMap<CreateOrderRequest, OrderEntity>();
            CreateMap<UpdateOrderRequest, OrderEntity>();

            // entity -> response
            CreateMap<OrderEntity, OrderResponse>();
        }
    }
}
