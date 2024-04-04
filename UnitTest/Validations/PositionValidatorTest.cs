using Application.DTOs;
using Application.Enums;
using Application.Validations;
using FluentValidation.TestHelper;
using Xunit;

namespace UnitTest.Validations
{
    public class PositionValidatorTest
    {
        private readonly PositionValidator _validator;

        public PositionValidatorTest()
        {
            _validator = new PositionValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PositionName_Invalid_ReturnNullOrEmpty(string positionName)
        {
            // Arrange
            var positionDto = new PositionDTO { PositionName = positionName };

            // Act
            var result = _validator.TestValidate(positionDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PositionName)
                .WithErrorMessage("Position name cannot be empty.");
        }

        [Fact]
        public void PositionName_Valid()
        {
            // Arrange
            var positionDto = new PositionDTO { PositionName = "Valid Position Name" };

            // Act
            var result = _validator.TestValidate(positionDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.PositionName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PositionDescription_Invalid_ReturnNullOrEmpty(string positionDescription)
        {
            // Arrange
            var positionDto = new PositionDTO { PositionDescription = positionDescription };

            // Act
            var result = _validator.TestValidate(positionDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PositionDescription)
                .WithErrorMessage("Position description cannot be empty.");
        }

        [Fact]
        public void PositionDescription_Valid()
        {
            // Arrange
            var positionDto = new PositionDTO { PositionDescription = "Valid Position Description" };

            // Act
            var result = _validator.TestValidate(positionDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.PositionDescription);
        }

        [Theory]
        [InlineData((Shiftdto)3)]
        public void Shift_Invalid_ReturnInEnum(Shiftdto shift)
        {
            // Arrange
            var positionDto = new PositionDTO { Shift = shift };

            // Act
            var result = _validator.TestValidate(positionDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Shift)
                .WithErrorMessage("Invalid shift value.");
        }

    }
}
