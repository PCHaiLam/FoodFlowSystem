using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Table;

namespace FoodFlowSystem.Validators.Table
{
    public class CreateTableValidator : AbstractValidator<CreateTableRequest>
    {
        public CreateTableValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required");
            RuleFor(x => x.Capacity).NotEmpty().WithMessage("Capacity is required");
        }
    }
}
