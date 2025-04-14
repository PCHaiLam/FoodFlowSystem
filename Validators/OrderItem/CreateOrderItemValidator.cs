using FluentValidation;
using FoodFlowSystem.DTOs.Requests.OrderItem;

namespace FoodFlowSystem.Validators.OrderItem
{
    public class CreateOrderItemValidator : AbstractValidator<CreateOrderItemRequest>
    {
        public CreateOrderItemValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductID is required");
            RuleFor(x => x.Quantity).NotEmpty().WithMessage("Quantity is required");
        }
    }
}
