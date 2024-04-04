using Application.DTOs;
using Application.Services.PaymentServices;
using AutoMapper;
using Domain.Contracts;
using Domain.Enums;
using Domain.Models;
using Moq;
using Xunit;

namespace UnitTest.Services.PaymentServices
{
    public class PaymentServiceTests
    {
        private static IMapper _mapper;
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
        private readonly PaymentService _paymentService;

        public PaymentServiceTests()
        {
            if (_mapper == null)
            {
                var configurationProvider = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Payment, PaymentDto>();
                    cfg.CreateMap<PaymentDto, Payment>();
                });

                _mapper = configurationProvider.CreateMapper();
            }
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
            _paymentService = new PaymentService(_paymentRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetPaymentByIdAsync_ReturnsPayment()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var payment = new Payment { Id = paymentId, };
            var paymentDto = new PaymentDto { Id = paymentId, };

            _paymentRepositoryMock.Setup(repo => repo.GetByIdAsync(paymentId)).ReturnsAsync(payment);

            // Act
            var result = await _paymentService.GetPaymentByIdAsync(paymentId);

            // Assert
            Assert.Equal(paymentDto.Id, result.Id);
        }

        [Fact]
        public async Task GetPaymentByIdAsync_WhenPaymentNotFound()
        {
            // Arrange
            var paymentId = Guid.NewGuid();

            _paymentRepositoryMock.Setup(repo => repo.GetByIdAsync(paymentId)).ReturnsAsync((Payment)null);

            // Act
            var result = await _paymentService.GetPaymentByIdAsync(paymentId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllPaymentsAsync_ReturnsAllPayment()
        {
            // Arrange
            var payments = new List<Payment>
            {
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
            };
            _paymentRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(payments);

            // Act
            var result = await _paymentService.GetAllPaymentsAsync();

            // Assert
            Assert.Equal(payments.Count, result.Count());
            foreach (var payment in payments)
            {
                Assert.Contains(result, p => p.Id == payment.Id);
            }
        }

        [Fact]
        public async Task GetAllPaymentsAsync_WhenNoPaymentsExist()
        {
            // Arrange
            _paymentRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync([]);

            // Act
            var result = await _paymentService.GetAllPaymentsAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task CreatePaymentAsync_WithValidData_PaymentIsCreated()
        {
            // Arrange
            var paymentDtoPayPal = new PaymentDto
            {
                PaymentMethod = PaymentMethod.PayPal,
                BillId = Guid.NewGuid(),
                Username = "TestUser",
                TotalAmount = 100
            };

            var paymentDtoStripe = new PaymentDto
            {
                PaymentMethod = PaymentMethod.Stripe,
                BillId = Guid.NewGuid(),
                Username = "TestUser",
                TotalAmount = 200
            };

            _paymentRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Payment>())).Returns(Task.CompletedTask);

            // PayPal Payment Act
            await _paymentService.AddPaymentAsync(paymentDtoPayPal);

            // Assert
            _paymentRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Payment>(p =>
                p.PaymentMethod == paymentDtoPayPal.PaymentMethod &&
                p.BillId == paymentDtoPayPal.BillId &&
                p.TotalAmount == (double)paymentDtoPayPal.TotalAmount)), Times.Once);

            // Stripe Payment Act
            await _paymentService.AddPaymentAsync(paymentDtoStripe);

            // Assert
            _paymentRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Payment>(p =>
                p.PaymentMethod == paymentDtoStripe.PaymentMethod &&
                p.BillId == paymentDtoStripe.BillId &&
                p.TotalAmount == (double)paymentDtoStripe.TotalAmount)), Times.Once);
        }

        [Fact]
        public async Task UpdatePaymentAsync_WithValidData_PaymentIsUpdated()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var paymentDto = new PaymentDto
            {
                PaymentMethod = PaymentMethod.PayPal,
                BillId = Guid.NewGuid(),
                Username = "TestUser",
                TotalAmount = 150
            };

            var existingPayment = new Payment
            {
                Id = paymentId,
                PaymentMethod = PaymentMethod.Stripe,
                BillId = Guid.NewGuid(),
                TotalAmount = 100
            };
            _paymentRepositoryMock.Setup(repo => repo.GetByIdAsync(paymentId)).ReturnsAsync(existingPayment);

            // Act
            await _paymentService.UpdatePaymentAsync(paymentId, paymentDto);

            // Assert
            _paymentRepositoryMock.Verify(repo => repo.UpdateAsync(paymentId, It.IsAny<Payment>()), Times.Once);
        }

        [Fact]
        public async Task DeletePaymentAsync_WithValidData_PaymentIsDeleted()
        {
            // Arrange
            var paymentId = Guid.NewGuid();

            _paymentRepositoryMock.Setup(repo => repo.DeleteAsync(paymentId)).Returns(Task.CompletedTask);

            // Act
            await _paymentService.DeletePaymentAsync(paymentId);

            // Assert
            _paymentRepositoryMock.Verify(repo => repo.DeleteAsync(paymentId), Times.Once);
        }
    }
}
