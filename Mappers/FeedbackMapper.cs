using AutoMapper;
using FoodFlowSystem.DTOs.Requests.Feedback;
using FoodFlowSystem.DTOs.Responses.Feedbacks;
using FoodFlowSystem.Entities.Feedback;

namespace FoodFlowSystem.Mappers
{
    public class FeedbackMapper : Profile
    {
        public FeedbackMapper()
        {
            //request to entity
            CreateMap<CreateFeedbackRequest, FeedbackEntity>();
            CreateMap<UpdateFeedbackRequest, FeedbackEntity>();

            //entity to response
            CreateMap<FeedbackEntity, FeedbackResponse>();
        }
    }
}
