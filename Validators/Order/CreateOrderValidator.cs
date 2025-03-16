using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Order;

namespace FoodFlowSystem.Validators.Order
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.TableID).NotEmpty();
            RuleFor(x => x.ReservationID).NotEmpty();
            RuleFor(x => x.OrderItems).NotEmpty();
        }
    }
}
