using Application.DTOs;
using Application.Validations;
using Domain.Models;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.Tests.Validations
{
    public class RoomTypeValidatorTest
    {
        private readonly RoomTypeValidator _validator;

        public RoomTypeValidatorTest()
        {
            _validator = new RoomTypeValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Type_Invalid_ReturnNullOrEmpty(string type)
        {
            // Arrange
            var roomTypeDto = new RoomTypeDto { Type = type };

            // Act
            var result = _validator.TestValidate(roomTypeDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Type)
                .WithErrorMessage("Room type is required.");
        }

        [Fact]
        public void Type_Return_Valid()
        {
            // Arrange
            var roomTypeDto = new RoomTypeDto { Type = "Valid Type" };

            // Act
            var result = _validator.TestValidate(roomTypeDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Type);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Description_Invalid_ReturnNullOrEmpty(string description)
        {
            // Arrange
            var roomTypeDto = new RoomTypeDto { Description = description };

            // Act
            var result = _validator.TestValidate(roomTypeDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage("Description is required.");
        }

        [Fact]
        public void Description_Return_Valid()
        {
            // Arrange
            var roomTypeDto = new RoomTypeDto { Description = "Valid Description" };

            // Act
            var result = _validator.TestValidate(roomTypeDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(-1.0)]
        public void Price_Return_Invalid(double price)
        {
            // Arrange
            var roomTypeDto = new RoomTypeDto { Price = price };

            // Act
            var result = _validator.TestValidate(roomTypeDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Price)
                .WithErrorMessage("Price must be greater than 0.");
        }

        [Fact]
        public void Price_Return_Valid()
        {
            // Arrange
            var roomTypeDto = new RoomTypeDto { Price = 100.00 };

            // Act
            var result = _validator.TestValidate(roomTypeDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Price);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Capacity_Return_Invalid(int capacity)
        {
            // Arrange
            var roomTypeDto = new RoomTypeDto { Capacity = capacity };

            // Act
            var result = _validator.TestValidate(roomTypeDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Capacity)
                .WithErrorMessage("Capacity must be greater than 0.");
        }

        [Fact]
        public void Capacity_Return_Valid()
        {
            // Arrange
            var roomTypeDto = new RoomTypeDto { Capacity = 2 };

            // Act
            var result = _validator.TestValidate(roomTypeDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Capacity);
        }
    }
}
