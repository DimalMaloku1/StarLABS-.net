using Domain.Enums;
using Domain.Models;
using FluentValidation;

namespace Application.Validations
{
    public class PaymentValidation : AbstractValidator<Payment>
    {
        public PaymentValidation()
        {
            RuleFor(payment => payment.PaymentMethod)
                .NotEmpty().WithMessage("Payment method is required.")
                .Must(method => method == PaymentMethod.Cash || method == PaymentMethod.Credit)
                .WithMessage("Payment method must be either 'Cash' or 'Credit'.");

            RuleFor(payment => payment.BillId)
                .NotEmpty().WithMessage("Bill ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Bill ID cannot be empty.");
        }
    }
}
