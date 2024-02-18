using FluentValidation;
using Domain.Models;

public class FeedbackValidator : AbstractValidator<Feedback>
{
    public FeedbackValidator()
    {
        RuleFor(feedback => feedback.Comment)
            .NotEmpty().WithMessage("Comment is required.")
            .MaximumLength(200).WithMessage("Comment must not exceed 200 characters.");

        RuleFor(feedback => feedback.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

        RuleFor(feedback => feedback.CreatedAt)
            .NotEmpty().WithMessage("Created at timestamp is required.");

      
    }
}
