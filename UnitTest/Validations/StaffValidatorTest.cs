using Application.DTOs;
using Application.Validations;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.Tests.Validations
{
    public class StaffValidatorTest
    {
        private readonly StaffValidator _validator;
        public StaffValidatorTest()
        {
            _validator = new StaffValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Department_Invalid_ReturnNullOrEmpty(string department)
        {
            // Arrange
            var staffDto = new StaffDTO { Department = department };

            // Act
            var result = _validator.TestValidate(staffDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Department)
                .WithErrorMessage("Department is required.");
        }

        [Fact]
        public void Department_Return_Valid()
        {
            // Arrange
            var staffDto = new StaffDTO { Department = "Valid Department" };

            // Act
            var result = _validator.TestValidate(staffDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Department);
        }

        [Fact]
        public void Department_InvalidReturn_ExceedsMaxLength()
        {
            // Arrange
            var staffDto = new StaffDTO { Department = new string('a', 101) }; 

            // Act
            var result = _validator.TestValidate(staffDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Department)
                .WithErrorMessage("Department cannot exceed 100 characters.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Salary_Invalid_Return(int salary)
        {
            // Arrange
            var staffDto = new StaffDTO { Salary = salary };

            // Act
            var result = _validator.TestValidate(staffDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Salary)
                .WithErrorMessage("Salary must be greater than 0.");
        }

        [Fact]
        public void Salary_Return_Valid()
        {
            // Arrange
            var staffDto = new StaffDTO { Salary = 1000 };

            // Act
            var result = _validator.TestValidate(staffDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Salary);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Address_Invalid_ReturnNullOrEmpty(string address)
        {
            // Arrange
            var staffDto = new StaffDTO { Address = address };

            // Act
            var result = _validator.TestValidate(staffDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Address)
                .WithErrorMessage("Address is required.");
        }

        [Fact]
        public void Address_Return_Valid()
        {
            // Arrange
            var staffDto = new StaffDTO { Address = "Valid Address" };

            // Act
            var result = _validator.TestValidate(staffDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Address);
        }

        [Fact]
        public void Address_Invalid_ExceedsMaxLength()
        {
            // Arrange
            var staffDto = new StaffDTO { Address = new string('a', 201) }; 

            // Act
            var result = _validator.TestValidate(staffDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Address)
                .WithErrorMessage("Address cannot exceed 200 characters.");
        }

        [Fact]
        public void PositionId_Invalid_ReturnNullOrEmpty()
        {
            // Arrange
            var staffDto = new StaffDTO(); 

            // Act
            var result = _validator.TestValidate(staffDto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PositionId)
                .WithErrorMessage("Position is required.");
        }
    }
}
