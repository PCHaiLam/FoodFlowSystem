using FluentValidation;
using FoodFlowSystem.DTOs.Requests.User;

namespace FoodFlowSystem.Validators.User
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.FullName);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Phone);
        }
    }
}
