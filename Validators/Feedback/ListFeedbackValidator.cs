using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Feedback;

namespace FoodFlowSystem.Validators.Feedback
{
    public class ListFeedbackValidator : AbstractValidator<CreatListFeedbacksRequest>
    {
        public ListFeedbackValidator()
        {
            RuleFor(x => x.ListFeedbacks)
                .NotEmpty().WithMessage("List of feedbacks cannot be empty.");
        }
    }
}
