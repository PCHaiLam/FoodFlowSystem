using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Category;

namespace FoodFlowSystem.Validators.Category
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Name).MaximumLength(100).WithMessage("Name must not exceed 100 characters");
            RuleFor(x => x.Description).MaximumLength(200).WithMessage("Description must not exceed 200 characters");
        }
    }
}
