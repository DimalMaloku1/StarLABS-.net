using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using FluentValidation;

namespace Application.Validations
{
    public class BillValidator : AbstractValidator<Bill>
    {
        public BillValidator()
        {
            RuleFor(x => x.TotalAmount)
                .NotEmpty().WithMessage("Total amount is required.")
                .GreaterThan(0).WithMessage("Total amount must be greater than 0.");

            RuleFor(x => x.BookingId)
                .NotEmpty().WithMessage("Booking ID is required.");
        }
    }
}
