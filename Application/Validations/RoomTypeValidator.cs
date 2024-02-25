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
    public class RoomTypeValidator : AbstractValidator<RoomTypeDto>
    {
        public RoomTypeValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Room type is required.")
                .MaximumLength(50).WithMessage("Room type cannot exceed 50 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(200).WithMessage("Description cannot exceed 200 characters.");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Capacity)
                .NotEmpty().WithMessage("Capacity is required.")
                .GreaterThan(0).WithMessage("Capacity must be greater than 0.");
        }
    }
}
