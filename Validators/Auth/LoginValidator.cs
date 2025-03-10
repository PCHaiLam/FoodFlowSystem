using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Auth;

namespace FoodFlowSystem.Validators.Auth
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
