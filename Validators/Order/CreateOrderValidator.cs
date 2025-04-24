using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Order;

namespace FoodFlowSystem.Validators.Order
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Số điện thoại phải được nhập.");
            RuleFor(x => x.Phone).Matches(@"^0[3|5|7|8|9][0-9]{8}$").WithMessage("Số điện thoại không hợp lệ.");
            RuleFor(x => x.Discount).GreaterThanOrEqualTo(0).WithMessage("Giảm giá không được âm.");
            RuleFor(x => x.Note).MaximumLength(500);
        }
    }
}
