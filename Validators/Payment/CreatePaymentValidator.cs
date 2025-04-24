using FluentValidation;
using FoodFlowSystem.DTOs.Requests.Payment;

namespace FoodFlowSystem.Validators.Payment
{
    public class CreatePaymentValidator : AbstractValidator<CreatePaymentRequest>
    {
        public CreatePaymentValidator()
        {
            //RuleFor(x => x.InvoiceId).NotEmpty().WithMessage("Invoice id is required");
            RuleFor(x => x.PaymentType).NotEmpty().WithMessage("PaymentType is required");
            RuleFor(x => x.PaymentMethod).NotEmpty().WithMessage("PaymentMethod is required");
        }
    }
}
