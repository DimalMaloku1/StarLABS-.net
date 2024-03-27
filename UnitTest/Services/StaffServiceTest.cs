using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Services.StaffServices;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace UnitTest.Services
{
    public class StaffServiceTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IStaffRepository> _staffRepositoryMock;
        private readonly Mock<IUserStore<AppUser>> _userStoreMock;
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly StaffService _staffService;
        public StaffServiceTest()
        {
            if (_mapper == null)
            {
                var mapperConfig = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Staff, StaffDTO>();
                    cfg.CreateMap<StaffDTO, Staff>();
                });
                _mapper = mapperConfig.CreateMapper();
            }
            _staffRepositoryMock = new Mock<IStaffRepository>();
            _userStoreMock = new Mock<IUserStore<AppUser>>();
            _userManagerMock = new Mock<UserManager<AppUser>>(_userStoreMock.Object, null, null, null, null, null, null, null, null);
            _staffService = new StaffService(_staffRepositoryMock.Object, _mapper, _userManagerMock.Object);
        }

        [Fact]
        public async Task GetAllStaffAsync_ReturnAllStaff()
        {
            // Arrange
            var staffEntities = new List<Staff>();
            _staffRepositoryMock.Setup(repo => repo.GetAllStaffAsync()).ReturnsAsync(staffEntities);
            var staffDTOs = staffEntities.Select(s => new StaffDTO()).ToList();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<IEnumerable<StaffDTO>>(staffEntities)).Returns(staffDTOs);

            // Act
            var result = await _staffService.GetAllStaffAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(staffDTOs.Count, result.Count());
        }

        [Fact]
        public async Task GetStaffByIdAsync_ReturnStaff()
        {
            // Arrange
            var staffId = Guid.NewGuid();
            var staffEntity = new Staff { Id = staffId };
            _staffRepositoryMock.Setup(repo => repo.GetStaffByIdAsync(staffId)).ReturnsAsync(staffEntity);
            var staffDTO = new StaffDTO { Id = staffId };

            // Act
            var result = await _staffService.GetStaffByIdAsync(staffId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(staffId, result.Id);
        }

        [Fact]
        public async Task AddStaffAsync_AddNewStaff()
        {
            // Arrange
            var staffDTO = new StaffDTO();
            var staffEntity = new Staff();

            // Act
            await _staffService.AddStaffAsync(staffDTO);

            // Assert
            _staffRepositoryMock.Verify(repo => repo.AddStaffAsync(It.IsAny<Staff>()), Times.Once);
        }

        [Fact]
        public async Task UpdateStaffAsync_UpdateExistingStaff()
        {
            // Arrange
            var staffId = Guid.NewGuid();
            var staffDTO = new StaffDTO { Id = staffId };
            var existingStaffEntity = new Staff { Id = staffId };
            _staffRepositoryMock.Setup(repo => repo.GetStaffByIdAsync(staffId)).ReturnsAsync(existingStaffEntity);

            // Act
            await _staffService.UpdateStaffAsync(staffId, staffDTO);

            // Assert
            _staffRepositoryMock.Verify(repo => repo.UpdateStaffAsync(existingStaffEntity), Times.Once);
        }

        [Fact]
        public async Task DeleteStaffAsync_DeleteStaff()
        {
            // Arrange
            var staffId = Guid.NewGuid();
            var existingStaffEntity = new Staff { Id = staffId };
            _staffRepositoryMock.Setup(repo => repo.GetStaffByIdAsync(staffId)).ReturnsAsync(existingStaffEntity);

            // Act
            await _staffService.DeleteStaffAsync(staffId);

            // Assert
            _staffRepositoryMock.Verify(repo => repo.DeleteStaffAsync(staffId), Times.Once);
        }

        [Fact]
        public async Task GetStaffByDepartmentAsync_ReturnStaffByDepartment()
        {
            // Arrange
            var department = "HR";
            var staffEntities = new List<Staff>();
            _staffRepositoryMock.Setup(repo => repo.GetStaffByDepartmentAsync(department)).ReturnsAsync(staffEntities);
            var staffDTOs = staffEntities.Select(s => new StaffDTO()).ToList();

            // Act
            var result = await _staffService.GetStaffByDepartmentAsync(department);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(staffDTOs.Count, result.Count());
        }

        [Fact]
        public async Task GetStaffByPositionAsync_ReturnStaffByPosition()
        {
            // Arrange
            var positionId = Guid.NewGuid();
            var staffEntities = new List<Staff>();
            _staffRepositoryMock.Setup(repo => repo.GetStaffByPositionAsync(positionId)).ReturnsAsync(staffEntities);
            var staffDTOs = staffEntities.Select(s => new StaffDTO()).ToList();

            // Act
            var result = await _staffService.GetStaffByPositionAsync(positionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(staffDTOs.Count, result.Count());
        }

        [Fact]
        public async Task GetStaffFullNameByStaffIdAsync_ReturnFullName()
        {
            // Arrange
            var staffId = Guid.NewGuid();
            var staff = new Staff { UserId = Guid.NewGuid() };
            _staffRepositoryMock.Setup(repo => repo.GetStaffByIdAsync(staffId)).ReturnsAsync(staff);
            var user = new AppUser { Id = staff.UserId, UserName = "John", UserLastname = "Doe" };
            _userManagerMock.Setup(manager => manager.FindByIdAsync(staff.UserId.ToString())).ReturnsAsync(user);

            // Act
            var result = await _staffService.GetStaffFullNameByStaffIdAsync(staffId);

            // Assert
            Assert.Equal($"{user.UserName} {user.UserLastname}", result);
        }
    }
}