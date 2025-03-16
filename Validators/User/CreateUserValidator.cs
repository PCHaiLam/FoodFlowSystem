using FluentValidation;
using FoodFlowSystem.DTOs.Requests.User;

namespace FoodFlowSystem.Validators.User
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.LastName);
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).MinimumLength(6);
            RuleFor(x => x.Phone).NotEmpty();
        }
    }
}
