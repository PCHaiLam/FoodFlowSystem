using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Auth;

namespace FoodFlowSystem.Validators.Auth
{
    public class RegisterValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.FullName).NotEmpty();
            RuleFor(x => x.Phone).NotEmpty();
        }
    }
}
