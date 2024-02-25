using Application.DTOs;
using FluentValidation;

namespace Application.Validations
{
    public class PaymentValidator : AbstractValidator<PaymentDto>
    {
        public PaymentValidator()
        {
            RuleFor(x => x.PaymentMethod)
                .NotEmpty().WithMessage("Please select a valid payment method.");

            RuleFor(x => x.BillId)
                .NotEmpty().WithMessage("Please select a valid bill.");
        }
    }
}