using Application.DTOs.AccountDTOs;
using FluentValidation;

namespace Application.Validations
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator() {
            RuleFor(dto => dto.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Email is not in the correct format.");

            RuleFor(dto => dto.Password)
               .NotEmpty().WithMessage("Password is required.")
               .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
               .Matches(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$")
               .WithMessage("Invalid email or password.");
        }
    
    }
}
