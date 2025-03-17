using AutoMapper;
using FoodFlowSystem.DTOs.Requests.Table;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Entities.Table;

namespace FoodFlowSystem.Mappers
{
    public class TableMapper : Profile
    {
        public TableMapper()
        {
            //request to entity
            CreateMap<CreateTableRequest, TableEntity>();
            CreateMap<UpdateTableRequest, TableEntity>();

            //entity to response
            CreateMap<TableEntity, TableResponse>();
        }
    }
}
