using Application.Services.LoggingServices;
using Domain.Contracts;
using Domain.Models;
using Moq;
using Xunit;

namespace UnitTest.Services.LoggingServices
{
    public class LoggingServiceTests
    {
        private readonly Mock<ILoggingRepository> _loggingRepositoryMock;
        private readonly LoggingService _loggingService;

        public LoggingServiceTests()
        {
            _loggingRepositoryMock = new Mock<ILoggingRepository>();
            _loggingService = new LoggingService(_loggingRepositoryMock.Object);
        }

        [Fact]
        public async Task LogActionAsync_WithValidData()
        {
            // Arrange
            var action = "Action";
            var entity = "Entity";
            var userName = "User";

            _loggingRepositoryMock.Setup(repo => repo.LogActionAsync(action, entity, userName)).Returns(Task.CompletedTask);

            // Act
            await _loggingService.LogActionAsync(action, entity, userName);

            // Assert
            _loggingRepositoryMock.Verify(repo => repo.LogActionAsync(action, entity, userName), Times.Once);
        }

        [Fact]
        public async Task GetAllLogsAsync_ReturnsAllLogs()
        {
            // Arrange
            var logs = new List<Log>
            {
                new Log { Id = Guid.NewGuid(), Action = "Action1", Entity = "Entity1", UserName = "User1", Timestamp = DateTime.UtcNow },
                new Log { Id = Guid.NewGuid(), Action = "Action2", Entity = "Entity2", UserName = "User2", Timestamp = DateTime.UtcNow }
            };
            _loggingRepositoryMock.Setup(repo => repo.GetAllLogsAsync()).ReturnsAsync(logs);

            // Act
            var result = await _loggingService.GetAllLogsAsync();

            // Assert
            Assert.Equal(logs.Count, result.Count());
            foreach (var log in logs)
            {
                Assert.Contains(result, l => l.Id == log.Id);
            }
        }

        [Fact]
        public async Task GetLogByIdAsync_ReturnsLog()
        {
            // Arrange
            var logId = Guid.NewGuid();
            var log = new Log { Id = logId, Action = "Action", Entity = "Entity", UserName = "User", Timestamp = DateTime.UtcNow };
            _loggingRepositoryMock.Setup(repo => repo.GetLogByIdAsync(logId)).ReturnsAsync(log);

            // Act
            var result = await _loggingService.GetLogByIdAsync(logId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(logId, result.Id);
        }

        [Fact]
        public async Task CreateLogAsync_WithValidData_LogIsCreated()
        {
            // Arrange
            var log = new Log { Action = "Action", Entity = "Entity", UserName = "User", Timestamp = DateTime.UtcNow };

            _loggingRepositoryMock.Setup(repo => repo.CreateLogAsync(log)).Returns(Task.CompletedTask);

            // Act
            await _loggingService.CreateLogAsync(log);

            // Assert
            _loggingRepositoryMock.Verify(repo => repo.CreateLogAsync(log), Times.Once);
        }

        [Fact]
        public async Task UpdateLogAsync_WithValidData_LogIsUpdated()
        {
            // Arrange
            var log = new Log { Id = Guid.NewGuid(), Action = "Action", Entity = "Entity", UserName = "User", Timestamp = DateTime.UtcNow };

            _loggingRepositoryMock.Setup(repo => repo.UpdateLogAsync(log)).Returns(Task.CompletedTask);

            // Act
            await _loggingService.UpdateLogAsync(log);

            // Assert
            _loggingRepositoryMock.Verify(repo => repo.UpdateLogAsync(log), Times.Once);
        }

        [Fact]
        public async Task DeleteLogAsync_WithValidData_LogIsDeleted()
        {
            // Arrange
            var logId = Guid.NewGuid();

            _loggingRepositoryMock.Setup(repo => repo.DeleteLogAsync(logId)).Returns(Task.CompletedTask);

            // Act
            await _loggingService.DeleteLogAsync(logId);

            // Assert
            _loggingRepositoryMock.Verify(repo => repo.DeleteLogAsync(logId), Times.Once);
        }

        [Fact]
        public async Task GetLogsByMonthYearAsync_ReturnsAllLogs()
        {
            // Arrange
            int month = 3;
            int year = 2024;
            var logs = new List<Log>
            {
                new Log { Id = Guid.NewGuid(), Action = "Action1", Entity = "Entity1", UserName = "User1", Timestamp = new DateTime(2024, 3, 15) },
                new Log { Id = Guid.NewGuid(), Action = "Action2", Entity = "Entity2", UserName = "User2", Timestamp = new DateTime(2024, 3, 20) },
                new Log { Id = Guid.NewGuid(), Action = "Action3", Entity = "Entity3", UserName = "User3", Timestamp = new DateTime(2024, 4, 5) }
            };
            _loggingRepositoryMock.Setup(repo => repo.GetLogsByMonthYearAsync(month, year)).ReturnsAsync(logs);

            // Act
            var result = await _loggingService.GetLogsByMonthYearAsync(month, year);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(logs.Count, result.Count());
        }

    }
}
