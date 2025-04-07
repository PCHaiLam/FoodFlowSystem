using AutoMapper;
using FoodFlowSystem.DTOs.Responses;
using FoodFlowSystem.Entities.Invoice;

namespace FoodFlowSystem.Mappers
{
    public class InvoiceMapper : Profile
    {
        public InvoiceMapper()
        {
            // request -> entity

            // entity -> response
            CreateMap<InvoiceEntity, InvoiceResponse>();
        }
    }
}
