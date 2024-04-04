using Application.DTOs;
using Application.Services.RoomServices;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Moq;
using Xunit;

namespace UnitTest.Services
{
    public class RoomServiceTest
    {
        private readonly Mock<IRoomRepository> _roomRepositoryMock;
        private readonly RoomServices _roomService;
        private readonly IMapper _mapper;

        public RoomServiceTest()
        {
            _roomRepositoryMock = new Mock<IRoomRepository>();
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Room, RoomDto>();
                cfg.CreateMap<RoomDto, Room>();
            }).CreateMapper();
            _roomService = new RoomServices(_roomRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAvailableRoomAsync_ReturnsAvailableRoom()
        {
            // Arrange
            var roomTypeId = Guid.NewGuid();
            var checkInDate = DateTime.UtcNow.AddDays(1);
            var checkOutDate = checkInDate.AddDays(2);

            var rooms = new List<Room>
            {
                new Room { Id = Guid.NewGuid(), RoomNumber = 101, RoomTypeId = roomTypeId, Bookings = new List<Booking>() },
                new Room { Id = Guid.NewGuid(), RoomNumber = 102, RoomTypeId = roomTypeId, Bookings = new List<Booking>() }
            };

            _roomRepositoryMock.Setup(repo => repo.GetRoomsByTypeAsync(roomTypeId)).ReturnsAsync(rooms);

            // Act
            var result = await _roomService.GetAvailableRoomAsync(roomTypeId, checkInDate, checkOutDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(rooms.First().Id, result.Id);
            Assert.Equal(rooms.First().RoomNumber, result.RoomNumber);
            Assert.Equal(rooms.First().RoomTypeId, result.RoomTypeId);
        }

        [Fact]
        public async Task GetAllRoomsAsync_ReturnsAllRooms()
        {
            // Arrange
            var rooms = new List<Room>
            {
                new Room { Id = Guid.NewGuid(), RoomNumber = 101, RoomTypeId = Guid.NewGuid() },
                new Room { Id = Guid.NewGuid(), RoomNumber = 102, RoomTypeId = Guid.NewGuid() }
            };

            _roomRepositoryMock.Setup(repo => repo.GetRoomsAsync()).ReturnsAsync(rooms);

            // Act
            var result = await _roomService.GetAllRoomsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(rooms.Count, result.Count());
            foreach (var room in rooms)
            {
                var roomDto = result.FirstOrDefault(r => r.Id == room.Id);
                Assert.NotNull(roomDto);
                Assert.Equal(room.RoomNumber, roomDto.RoomNumber);
                Assert.Equal(room.RoomTypeId, roomDto.RoomTypeId);
            }
        }

        [Fact]
        public async Task CreateAsync_AddsNewRoom()
        {
            // Arrange
            var newRoomDto = new RoomDto { RoomNumber = 103, RoomTypeId = Guid.NewGuid() };

            // Act
            var result = await _roomService.CreateAsync(newRoomDto);

            // Assert
            _roomRepositoryMock.Verify(repo => repo.Add(It.IsAny<Room>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(newRoomDto.RoomNumber, result.RoomNumber);
            Assert.Equal(newRoomDto.RoomTypeId, result.RoomTypeId);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesRoom()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var updatedRoomDto = new RoomDto { Id = roomId, RoomNumber = 104, RoomTypeId = Guid.NewGuid() };
            var existingRoom = new Room { Id = roomId, RoomNumber = 103, RoomTypeId = Guid.NewGuid() };
            _roomRepositoryMock.Setup(repo => repo.GetRoomByIdAsync(roomId)).ReturnsAsync(existingRoom);

            // Act
            await _roomService.UpdateAsync(roomId, updatedRoomDto);

            // Assert
            Assert.Equal(updatedRoomDto.RoomNumber, existingRoom.RoomNumber);
            Assert.Equal(updatedRoomDto.RoomTypeId, existingRoom.RoomTypeId);
            _roomRepositoryMock.Verify(repo => repo.UpdateAsync(roomId, It.IsAny<Room>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeletesRoom()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var existingRoom = new Room { Id = roomId, RoomNumber = 101, RoomTypeId = Guid.NewGuid() };

            _roomRepositoryMock.Setup(repo => repo.GetRoomByIdAsync(roomId)).ReturnsAsync(existingRoom);

            // Act
            await _roomService.DeleteAsync(roomId);

            // Assert
            _roomRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Room>()), Times.Once);
        }

        [Fact]
        public async Task GetRoomByIdAsync_ReturnsCorrectRoom()
        {
            // Arrange
            var roomId = Guid.NewGuid();
            var room = new Room { Id = roomId, RoomNumber = 101, RoomTypeId = Guid.NewGuid() };
            _roomRepositoryMock.Setup(repo => repo.GetRoomByIdAsync(roomId)).ReturnsAsync(room);

            // Act
            var result = await _roomService.GetRoomByIdAsync(roomId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(room.RoomNumber, result.RoomNumber);
            Assert.Equal(room.RoomTypeId, result.RoomTypeId);
        }
    }
}
