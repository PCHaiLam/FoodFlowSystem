using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Order;

namespace FoodFlowSystem.Validators.Order
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderRequest>
    {
        public UpdateOrderValidator()
        {
            RuleFor(x => x.OrderID).NotEmpty();
            RuleFor(x => x.OrderItems).NotEmpty();
        }
    }
}
