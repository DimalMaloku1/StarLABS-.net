using Application.DTOs;
using Application.Services.DailyTaskServices;
using Application.Services.EmailServices;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Moq;
using Xunit;

namespace UnitTest.Services
{
    public class DailyTaskServiceTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IDailyTaskRepository> _dailyTaskRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IStaffRepository> _staffRepositoryMock;
        private readonly DailyTaskService _dailyTaskService;

        public DailyTaskServiceTest()
        {
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DailyTaskDto, DailyTask>();
                cfg.CreateMap<DailyTask, DailyTaskDto>()
                    .ForMember(dest => dest.Staffs, opt => opt.MapFrom(src => new List<StaffDTO> { new StaffDTO { UserId = src.Staff.UserId } }));
            }).CreateMapper();

            _dailyTaskRepositoryMock = new Mock<IDailyTaskRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _staffRepositoryMock = new Mock<IStaffRepository>();
            _dailyTaskService = new DailyTaskService(_dailyTaskRepositoryMock.Object, _mapper, _emailServiceMock.Object, _staffRepositoryMock.Object); // Pass _staffRepositoryMock.Object
        }

        [Fact]
        public async Task CreateAsync_AddDailyTask()
        {
            // Arrange
            var dailyTaskDto = new DailyTaskDto
            {
                Id = Guid.NewGuid(),
                Name = "Test Task",
                Date = DateTime.Now,
                StaffId = Guid.NewGuid()
            };
            _dailyTaskRepositoryMock.Setup(repo => repo.Add(It.IsAny<DailyTask>())).Returns(Task.CompletedTask);

            // Act
            var result = await _dailyTaskService.CreateAsync(dailyTaskDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dailyTaskDto.Id, result.Id);
            _dailyTaskRepositoryMock.Verify(repo => repo.Add(It.IsAny<DailyTask>()), Times.Once);
        }


        [Fact]
        public async Task DeleteAsync_DeleteDailyTask()
        {
            // Arrange
            var dailyTaskId = Guid.NewGuid();
            _dailyTaskRepositoryMock.Setup(repo => repo.GetDailyTaskByIdAsync(dailyTaskId)).ReturnsAsync(new DailyTask());

            // Act
            await _dailyTaskService.DeleteAsync(dailyTaskId);

            // Assert
            _dailyTaskRepositoryMock.Verify(repo => repo.Delete(It.IsAny<DailyTask>()), Times.Once);
        }

        [Fact]
        public async Task GetAllDailyTasksAsync_ReturnAllDailyTasks()
        {
            // Arrange
            var dailyTasks = new List<DailyTask> { new DailyTask(), new DailyTask() };
            _dailyTaskRepositoryMock.Setup(repo => repo.GetDailyTasksAsync()).ReturnsAsync(dailyTasks);

            // Act
            var result = await _dailyTaskService.GetAllDailyTasksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dailyTasks.Count, result.Count());
        }

        [Fact]
        public async Task GetDailyTaskByIdAsync_ReturnDailyTask()
        {
            // Arrange
            var dailyTaskId = Guid.NewGuid();
            _dailyTaskRepositoryMock.Setup(repo => repo.GetDailyTaskByIdAsync(dailyTaskId)).ReturnsAsync(new DailyTask { Id = dailyTaskId });

            // Act
            var result = await _dailyTaskService.GetDailyTaskByIdAsync(dailyTaskId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dailyTaskId, result.Id);
        }

        [Fact]
        public async Task UpdateAsync_UpdateDailyTask()
        {
            // Arrange
            var dailyTaskId = Guid.NewGuid();
            var dailyTaskDto = new DailyTaskDto { Id = dailyTaskId };
            var existingDailyTask = new DailyTask { Id = dailyTaskId };
            _dailyTaskRepositoryMock.Setup(repo => repo.GetDailyTaskByIdAsync(dailyTaskId)).ReturnsAsync(existingDailyTask);

            // Act
            await _dailyTaskService.UpdateAsync(dailyTaskId, dailyTaskDto);

            // Assert
            _dailyTaskRepositoryMock.Verify(repo => repo.UpdateAsync(dailyTaskId, It.IsAny<DailyTask>()), Times.Once);
        }

        [Fact]
        public async Task GetDailyTasksByStatusAsync_ReturnDailyTasksByStatus()
        {
            // Arrange
            var status = "InProgress";
            var dailyTasks = new List<DailyTask> { new DailyTask { Status = status }, new DailyTask { Status = status } };
            _dailyTaskRepositoryMock.Setup(repo => repo.GetDailyTasksAsync()).ReturnsAsync(dailyTasks);

            // Act
            var result = await _dailyTaskService.GetDailyTasksByStatusAsync(status);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dailyTasks.Count, result.Count());
            Assert.All(result, task => Assert.Equal(status, task.Status));
        }

        [Fact]
        public async Task GetDailyTasksByDateAsync_ReturnDailyTasksByDate()
        {
            // Arrange
            var date = DateTime.Today;
            var dailyTasks = new List<DailyTask> { new DailyTask { Date = date }, new DailyTask { Date = date } };
            _dailyTaskRepositoryMock.Setup(repo => repo.GetDailyTasksAsync()).ReturnsAsync(dailyTasks);

            // Act
            var result = await _dailyTaskService.GetDailyTasksByDateAsync(date);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dailyTasks.Count, result.Count());
            Assert.All(result, task => Assert.Equal(date, task.Date.Date));
        }

        [Fact]
        public async Task GetDailyTasksByStaffIdAsync_ReturnDailyTasksByStaffId()
        {
            // Arrange
            var staffId = Guid.NewGuid();
            var dailyTasks = new List<DailyTask> { new DailyTask { StaffId = staffId }, new DailyTask { StaffId = staffId } };
            _dailyTaskRepositoryMock.Setup(repo => repo.GetDailyTasksAsync()).ReturnsAsync(dailyTasks);

            // Act
            var result = await _dailyTaskService.GetDailyTasksByStaffIdAsync(staffId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dailyTasks.Count, result.Count());
            Assert.All(result, task => Assert.Equal(staffId, task.StaffId));
        }

        [Fact]
        public async Task GetDailyTasksByUserIdAsync_ReturnDailyTasksByUserId()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var dailyTasks = new List<DailyTask> { new DailyTask { Staff = new Staff { UserId = userId } }, new DailyTask { Staff = new Staff { UserId = userId } } };
            _dailyTaskRepositoryMock.Setup(repo => repo.GetDailyTasksAsync()).ReturnsAsync(dailyTasks);

            // Act
            var result = await _dailyTaskService.GetDailyTasksByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dailyTasks.Count, result.Count());
            Assert.All(result, task => Assert.Equal(userId, task.Staffs.FirstOrDefault()?.UserId));
        }
    }
}