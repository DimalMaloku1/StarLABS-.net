using Application.DTOs;
using Application.Validations;
using FluentValidation.TestHelper;
using Xunit;

namespace UnitTest.Validators
{
    public class RoomValidatorTest
    {
        private readonly RoomValidator _validator;

        public RoomValidatorTest()
        {
            _validator = new RoomValidator();
        }

        [Theory]
        [InlineData(null)]
        public void RoomNumber_Null_ReturnValidationError(int roomNumber)
        {
            // Arrange
            var roomDto = new RoomDto { RoomNumber = roomNumber };

            // Act
            var result = _validator.TestValidate(roomDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RoomNumber)
                  .WithErrorMessage("Room number is required.");
        }

        [Theory]
        [InlineData(0)] 
        [InlineData(-1)] 
        public void RoomNumber_Invalid_ReturnGreaterThanZero(int roomNumber)
        {
            // Arrange
            var roomDto = new RoomDto { RoomNumber = roomNumber };

            // Act
            var result = _validator.TestValidate(roomDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RoomNumber)
                .WithErrorMessage("Room number must be greater than 0.");
        }

        [Fact]
        public void RoomNumber_Valid()
        {
            // Arrange
            var roomDto = new RoomDto { RoomNumber = 1 };

            // Act
            var result = _validator.TestValidate(roomDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.RoomNumber);
        }

    }
}
