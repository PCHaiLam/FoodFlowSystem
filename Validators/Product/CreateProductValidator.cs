using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Product;

namespace FoodFlowSystem.Validators.Product
{
    public class CreateProductValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required");
            RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required");
            RuleFor(x => x.UrlImage).NotEmpty().WithMessage("ImageUrl is required");
            RuleFor(x => x.CategoryID).NotEmpty().WithMessage("CategoryID is required");
        }
    }
}
