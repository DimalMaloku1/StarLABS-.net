using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Models;
using FluentValidation;

namespace Application.Validations
{
    public class BillValidator : AbstractValidator<BillDto>
    {
        public BillValidator()
        {
            RuleFor(x => x.BookingId).NotEmpty()
                .WithMessage("Booking Id cannot be null");
        }
    }
}
