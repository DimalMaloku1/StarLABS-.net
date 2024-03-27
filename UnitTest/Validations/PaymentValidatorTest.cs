using Application.DTOs;
using Application.Validations;
using FluentValidation.TestHelper;
using Xunit;

namespace UnitTest.Validations
{
    public class PaymentValidatorTest
    {
        [Fact]
        public void PaymentMethod_IsNotSelected()
        {
            // Arrange
            var validator = new PaymentValidator();
            var paymentDto = new PaymentDto();

            // Act
            var validationResult = validator.TestValidate(paymentDto);

            // Assert
            validationResult.ShouldHaveValidationErrorFor(x => x.PaymentMethod)
                            .WithErrorMessage("Please select a payment method.");
        }

        [Fact]
        public void PaymentMethod_IsSelected()
        {
            // Arrange
            var validator = new PaymentValidator();
            var paymentDto = new PaymentDto { PaymentMethod = Domain.Enums.PaymentMethod.PayPal };

            // Act
            var validationResult = validator.TestValidate(paymentDto);

            // Assert
            validationResult.ShouldNotHaveValidationErrorFor(x => x.PaymentMethod);
        }

    }
}
