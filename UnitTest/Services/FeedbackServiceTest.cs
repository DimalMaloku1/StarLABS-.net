using Application.DTOs;
using Application.Services.FeedbackServices;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Services.FeedbackServices
{
    public class FeedbackServiceTests
    {
        private static IMapper _mapper;
        private readonly Mock<IFeedbackRepository> _feedbackRepositoryMock;
        private readonly FeedbackService _feedbackService;
        private IOptions<IdentityOptions> optionsAccessor;

        public FeedbackServiceTests()
        {
            if (_mapper == null)
            {
                var configurationProvider = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Feedback, FeedbackDto>();
                    cfg.CreateMap<FeedbackDto, Feedback>();
                });

                _mapper = configurationProvider.CreateMapper();
            }
            _feedbackRepositoryMock = new Mock<IFeedbackRepository>();
            var userStoreMock = new Mock<IUserStore<AppUser>>();
            var identityOptions = new IdentityOptions();
            var passwordHasher = new PasswordHasher<AppUser>();
            var userValidators = new List<IUserValidator<AppUser>>();
            var passwordValidators = new List<IPasswordValidator<AppUser>>();
            var keyNormalizer = new UpperInvariantLookupNormalizer();
            var errors = new IdentityErrorDescriber();
            var services = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<AppUser>>>();

            var userManager = new UserManager<AppUser>(
                userStoreMock.Object,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services.Object,
                logger.Object);

            if (_mapper == null)
            {
                var configurationProvider = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Feedback, FeedbackDto>();
                    cfg.CreateMap<FeedbackDto, Feedback>();
                });

                _mapper = configurationProvider.CreateMapper();
            }

            _feedbackService = new FeedbackService(_feedbackRepositoryMock.Object, _mapper, userManager);
        }

        [Fact]
        public async Task GetFeedbackByIdAsync_ReturnsCorrectFeedbackDto()
        {
            // Arrange
            var feedbackId = Guid.NewGuid();
            var feedback = new Feedback { Id = feedbackId };
            var feedbackDto = new FeedbackDto { Id = feedbackId };

            _feedbackRepositoryMock.Setup(repo => repo.GetFeedbackByIdAsync(feedbackId)).ReturnsAsync(feedback);

            // Act
            var result = await _feedbackService.GetFeedbackByIdAsync(feedbackId);

            // Assert
            Assert.Equal(feedbackDto.Id, result.Id);
        }

        [Fact]
        public async Task GetFeedbackByIdAsync_ReturnsNotFound_WhenFeedbackNotFound()
        {
            // Arrange
            var feedbackId = Guid.NewGuid();

            _feedbackRepositoryMock.Setup(repo => repo.GetFeedbackByIdAsync(feedbackId)).ReturnsAsync((Feedback)null);

            // Act
            var result = await _feedbackService.GetFeedbackByIdAsync(feedbackId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_CreatesNewFeedback()
        {
            // Arrange
            var feedbackDto = new FeedbackDto { Comment = "Test Comment", Rating = 4, UserId = Guid.NewGuid() };

            // Act
            var result = await _feedbackService.CreateAsync(feedbackDto);

            // Assert
            _feedbackRepositoryMock.Verify(repo => repo.Add(It.IsAny<Feedback>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(feedbackDto.Comment, result.Comment);
            Assert.Equal(feedbackDto.Rating, result.Rating);
            Assert.Equal(feedbackDto.UserId, result.UserId);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesFeedback()
        {
            // Arrange
            var feedbackId = Guid.NewGuid();
            var feedbackDto = new FeedbackDto { Id = feedbackId, Comment = "Updated Comment", Rating = 5 };
            var existingFeedback = new Feedback { Id = feedbackId, Comment = "Original Comment", Rating = 4 };

            _feedbackRepositoryMock.Setup(repo => repo.GetFeedbackByIdAsync(feedbackId)).ReturnsAsync(existingFeedback);

            // Act
            await _feedbackService.UpdateAsync(feedbackId, feedbackDto);

            // Assert
            Assert.Equal(feedbackDto.Comment, existingFeedback.Comment);
            Assert.Equal(feedbackDto.Rating, existingFeedback.Rating);
            _feedbackRepositoryMock.Verify(repo => repo.Update(It.IsAny<Feedback>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeletesFeedback()
        {
            // Arrange
            var feedbackId = Guid.NewGuid();
            var existingFeedback = new Feedback { Id = feedbackId };

            _feedbackRepositoryMock.Setup(repo => repo.GetFeedbackByIdAsync(feedbackId)).ReturnsAsync(existingFeedback);

            // Act
            await _feedbackService.DeleteAsync(feedbackId);

            // Assert
            _feedbackRepositoryMock.Verify(repo => repo.Delete(existingFeedback), Times.Once);
        }

        [Fact]
        public async Task GetTopRatedFeedbacksAsync_ReturnsTopRatedFeedbacks()
        {
            // Arrange
            var feedbacks = new List<Feedback>
    {
        new Feedback { Id = Guid.NewGuid(), Comment = "Great experience", Rating = 5 },
        new Feedback { Id = Guid.NewGuid(), Comment = "Average service", Rating = 3 },
        new Feedback { Id = Guid.NewGuid(), Comment = "Excellent", Rating = 4 },
        new Feedback { Id = Guid.NewGuid(), Comment = "Poor service", Rating = 2 },
        new Feedback { Id = Guid.NewGuid(), Comment = "Fantastic", Rating = 5 }
    };
            _feedbackRepositoryMock.Setup(repo => repo.GetFeedbacksAsync()).ReturnsAsync(feedbacks);

            var count = 3;

            // Act
            var result = await _feedbackService.GetTopRatedFeedbacksAsync(count);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(count, result.Count());

            var expectedTopRatedFeedbacks = feedbacks.OrderByDescending(f => f.Rating).Take(count);
            foreach (var expectedFeedback in expectedTopRatedFeedbacks)
            {
                var actualFeedback = result.FirstOrDefault(f => f.Id == expectedFeedback.Id);
                Assert.NotNull(actualFeedback);
                Assert.Equal(expectedFeedback.Comment, actualFeedback.Comment);
                Assert.Equal(expectedFeedback.Rating, actualFeedback.Rating);
            }
        }

    }
}
