using AutoMapper;
using FoodFlowSystem.DTOs.Requests.Payment;
using FoodFlowSystem.DTOs.Responses.Payments;
using FoodFlowSystem.Entities.Payment;

namespace FoodFlowSystem.Mappers
{
    public class PaymentMapper : Profile
    {
        public PaymentMapper()
        {
            //request to entity
            CreateMap<CreatePaymentRequest, PaymentEntity>();
            //CreateMap<UpdatePaymentRequest, PaymentEntity>();

            //entity to response
            CreateMap<PaymentEntity, PaymentResponse>();

        }
    }
}
