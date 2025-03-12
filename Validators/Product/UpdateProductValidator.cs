using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Product;

namespace FoodFlowSystem.Validators.Product
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
            RuleFor(x => x.Quantity).NotEmpty().WithMessage("Quantity is required");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
            RuleFor(x => x.CategoryID).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.CategoryID).GreaterThan(0).WithMessage("Category must be greater than 0");
        }
    }
}
