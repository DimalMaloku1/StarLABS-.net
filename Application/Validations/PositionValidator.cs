using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using FluentValidation;

namespace Application.Validations
{
    public class PositionValidator : AbstractValidator<PositionDTO>
    {
        public PositionValidator() 
        {
            RuleFor(position => position.PositionName)
                .NotEmpty().WithMessage("Position name cannot be empty.")
                .MaximumLength(50).WithMessage("Position name must not exceed 50 characters.");

            RuleFor(position => position.PositionDescription)
                .NotEmpty().WithMessage("Position description cannot be empty.");

            RuleFor(position => position.Shift)
                .IsInEnum().WithMessage("Invalid shift value.");

        }
    }
}
