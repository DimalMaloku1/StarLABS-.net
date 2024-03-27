using Application.DTOs;
using FluentValidation;

namespace Application.Validations
{
    public class PaymentValidator : AbstractValidator<PaymentDto>
    {
        public PaymentValidator()
        {
            RuleFor(x => x.PaymentMethod)
                .NotEmpty().WithMessage("Please select a payment method.");
        }
    }
}