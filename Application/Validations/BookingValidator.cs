using Domain.Models;
using FluentValidation;

namespace Application.Validations
{
    public class BookingValidator : AbstractValidator<Booking>
    {
        public BookingValidator()
        {
            RuleFor(x => x.TotalPrice)
                .NotEmpty().WithMessage("Total price is required.")
                .GreaterThan(0).WithMessage("Total price must be greater than 0.");

            RuleFor(x => x.CheckInDate)
                .NotEmpty().WithMessage("Check-in date is required.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Check-in date must be today or later.");

            RuleFor(x => x.CheckOutDate)
                .NotEmpty().WithMessage("Check-out date is required.")
                .GreaterThanOrEqualTo(x => x.CheckInDate).WithMessage("Check-out date must be after check-in date.");

            RuleFor(x => x.RoomId)
                .NotEmpty().WithMessage("Room ID is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
