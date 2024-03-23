using Domain.Models;
using Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    public class RoomValidator : AbstractValidator<RoomDto>
    {
        public RoomValidator()
        {
            RuleFor(x => x.RoomNumber)
                .NotEmpty().WithMessage("Room number is required.")
                .GreaterThan(0).WithMessage("Room number must be greater than 0.");

        }
    }
}
