using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Order;

namespace FoodFlowSystem.Validators.Order
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required");
            RuleFor(x => x.Note).MaximumLength(500);
        }
    }
}
