using FluentValidation;
using FoodFlowSystem.DTOs.Requests.OrderItem;

namespace FoodFlowSystem.Validators.OrderItem
{
    public class UpdateOrderItemValidator : AbstractValidator<UpdateOrderItemRequest>
    {
        public UpdateOrderItemValidator()
        {
            RuleFor(x => x.ProductID).NotEmpty().WithMessage("ProductID is required");
            RuleFor(x => x.Quantity).NotEmpty().WithMessage("Quantity is required");
        }
    }
}
