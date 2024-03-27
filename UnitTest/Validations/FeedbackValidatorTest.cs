using Domain.Models;
using FluentValidation.TestHelper;
using Xunit;

namespace UnitTest.Validators
{
    public class FeedbackValidatorTests
    {
        private readonly FeedbackValidator _validator;

        public FeedbackValidatorTests()
        {
            _validator = new FeedbackValidator();
        }

        [Fact]
        public void Comment_Should_Have_Error_When_Empty()
        {
            // Arrange
            var feedback = new Feedback { Comment = "" };

            // Act
            var result = _validator.TestValidate(feedback);

            // Assert
            result.ShouldHaveValidationErrorFor(f => f.Comment)
                  .WithErrorMessage("Comment is required.");
        }

        [Fact]
        public void Comment_Should_Have_Error_When_Exceeds_Max_Length()
        {
            // Arrange
            var feedback = new Feedback { Comment = new string('x', 201) };

            // Act
            var result = _validator.TestValidate(feedback);

            // Assert
            result.ShouldHaveValidationErrorFor(f => f.Comment)
                  .WithErrorMessage("Comment must not exceed 200 characters.");
        }

        [Fact]
        public void Rating_Should_Have_Error_When_Out_Of_Range()
        {
            // Arrange
            var feedback = new Feedback { Rating = 6 };

            // Act
            var result = _validator.TestValidate(feedback);

            // Assert
            result.ShouldHaveValidationErrorFor(f => f.Rating)
                  .WithErrorMessage("Rating must be between 1 and 5.");
        }

        [Fact]
        public void CreatedAt_Should_Have_Error_When_Empty()
        {
            // Arrange
            var feedback = new Feedback { CreatedAt = default };

            // Act
            var result = _validator.TestValidate(feedback);

            // Assert
            result.ShouldHaveValidationErrorFor(f => f.CreatedAt)
                  .WithErrorMessage("Created at timestamp is required.");
        }

   
    }
}
