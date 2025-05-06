using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Feedback;

namespace FoodFlowSystem.Validators.Feedback
{
    public class CreateFeedbackValidator : AbstractValidator<CreateFeedbackRequest>
    {
        public CreateFeedbackValidator()
        {
            //add withmessage
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
            RuleFor(x => x.Rating).InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");
        }
    }
}
