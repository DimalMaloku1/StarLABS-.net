using Application.DTOs;
using FluentValidation;

namespace Application.Validations
{
    public class BookingValidator : AbstractValidator<BookingDto>
    {
        public BookingValidator()
        {
        

            RuleFor(x => x.CheckInDate)
                .NotEmpty().WithMessage("Check-in date is required.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Check-in date must be today or later.");

            RuleFor(x => x.CheckOutDate)
                .NotEmpty().WithMessage("Check-out date is required.")
                .GreaterThanOrEqualTo(x => x.CheckInDate).WithMessage("Check-out date must be after check-in date.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User is required.");
            RuleFor(x => x.RoomTypeId)
                .NotEmpty().WithMessage("RoomType is required.");
        }
    }
}
