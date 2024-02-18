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
                .NotEmpty().WithMessage("Payment method is required.");


            RuleFor(payment => payment.BillId)
                .NotEmpty().WithMessage("Bill ID is required.");
        }
    }
}
