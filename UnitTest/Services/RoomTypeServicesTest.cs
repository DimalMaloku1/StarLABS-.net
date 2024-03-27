using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Services.PaymentServices;
using Application.Services.RoomTypeServices;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

public class RoomTypeServicesTests
{
    private static IMapper _mapper;
    private readonly Mock<IRoomTypeRepository> _roomTypeRepositoryMock;
    private readonly RoomTypeServices _roomTypeServices;

    public RoomTypeServicesTests()
    {
        if (_mapper == null)
        {
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RoomType, RoomTypeDto>();
                cfg.CreateMap<RoomTypeDto, RoomType>();
            });

            _mapper = configurationProvider.CreateMapper();
        }
        _roomTypeRepositoryMock = new Mock<IRoomTypeRepository>();
        _roomTypeServices = new RoomTypeServices(_roomTypeRepositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task GetAllRoomTypesAsync_ReturnListOfRoomTypeDto()
    {
        // Arrange
        var roomTypes = new List<RoomType>
    {
        new RoomType { Id = Guid.NewGuid(), Type = "Single Room" },
        new RoomType { Id = Guid.NewGuid(), Type = "Double Room" }
    };
        _roomTypeRepositoryMock.Setup(repo => repo.GetRoomTypesAsync()).ReturnsAsync(roomTypes);

        // Act
        var result = await this._roomTypeServices.GetAllRoomTypesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(roomTypes.Count, result.Count());
    }

    [Fact]
    public async Task GetRoomTypeByIdAsync_ReturnRoomTypeDto()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var roomType = new RoomType { Id = roomId, Type = "Single Room" };
        var roomTypeDto = new RoomTypeDto { Id = roomId, Type = "Single Room" };
        var mockRepository = new Mock<IRoomTypeRepository>();
        mockRepository.Setup(repo => repo.GetRoomTypeByIdAsync(roomId)).ReturnsAsync(roomType);
        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(mapper => mapper.Map<RoomTypeDto>(roomType)).Returns(roomTypeDto);
        var service = new RoomTypeServices(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetRoomTypeByIdAsync(roomId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(roomId, result.Id);
        Assert.Equal("Single Room", result.Type);
    }

    [Fact]
    public async Task CreateAsync_CreateNewRoomType()
    {
        // Arrange
        var roomTypeDto = new RoomTypeDto { Type = "New Room" };
        var photos = new List<IFormFile>(); 
        var mockRepository = new Mock<IRoomTypeRepository>();
        mockRepository.Setup(repo => repo.Add(It.IsAny<RoomType>())).Callback<RoomType>(roomType =>
        {
            roomType.Id = Guid.NewGuid();
        });
        var service = new RoomTypeServices(mockRepository.Object, _mapper);

        // Act
        var result = await service.CreateAsync(roomTypeDto, photos);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(roomTypeDto.Type, result.Type);
    }

    [Fact]
    public async Task UpdateAsync_UpdateExistingRoomType()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var roomTypeDto = new RoomTypeDto { Id = roomId, Type = "Updated Room" };
        var existingRoomType = new RoomType { Id = roomId, Type = "Existing Room" };
        var mockRepository = new Mock<IRoomTypeRepository>();
        mockRepository.Setup(repo => repo.GetRoomTypeByIdAsync(roomId)).ReturnsAsync(existingRoomType);
        var mockMapper = new Mock<IMapper>();
        var service = new RoomTypeServices(mockRepository.Object, mockMapper.Object);

        // Act
        await service.UpdateAsync(roomId, roomTypeDto);

        // Assert
        mockRepository.Verify(repo => repo.UpdateAsync(roomId, It.IsAny<RoomType>()), Times.Once);
        mockMapper.Verify(mapper => mapper.Map(roomTypeDto, existingRoomType), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeleteRoomType()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var roomType = new RoomType { Id = roomId, Type = "Room to delete" };
        var mockRepository = new Mock<IRoomTypeRepository>();
        mockRepository.Setup(repo => repo.GetRoomTypeByIdAsync(roomId)).ReturnsAsync(roomType);
        var mockMapper = new Mock<IMapper>();
        var service = new RoomTypeServices(mockRepository.Object, mockMapper.Object);

        // Act
        await service.DeleteAsync(roomId);

        // Assert
        mockRepository.Verify(repo => repo.Delete(roomType), Times.Once);
    }
}
