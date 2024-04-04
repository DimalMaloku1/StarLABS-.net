using Application.DTOs;
using Application.Services.PositionServices;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Moq;
using Xunit;

namespace UnitTest.Services
{
    public class PositionServiceTest
    {
        private readonly Mock<IPositionRepository> _positionRepositoryMock;
        private readonly PositionServices _positionService;
        private readonly IMapper _mapper;

        public PositionServiceTest()
        {
            _positionRepositoryMock = new Mock<IPositionRepository>();
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Position, PositionDTO>();
                cfg.CreateMap<PositionDTO, Position>();
            }).CreateMapper();
            _positionService = new PositionServices(_positionRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllPositionsAsync_ReturnsAllPositions()
        {
            // Arrange
            var positions = new List<Position>
            {
                new Position { Id = Guid.NewGuid(), PositionName = "Position 1" },
                new Position { Id = Guid.NewGuid(), PositionName = "Position 2" }
            };
            _positionRepositoryMock.Setup(repo => repo.GetAllPositionsAsync()).ReturnsAsync(positions);

            // Act
            var result = await _positionService.GetAllPositionsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(positions.Count, result.Count());
            foreach (var position in positions)
            {
                var positionDto = result.FirstOrDefault(p => p.Id == position.Id);
                Assert.NotNull(positionDto);
                Assert.Equal(position.PositionName, positionDto.PositionName);
            }
        }

        [Fact]
        public async Task GetPositionByIdAsync_ReturnsCorrectPosition()
        {
            // Arrange
            var positionId = Guid.NewGuid();
            var position = new Position { Id = positionId, PositionName = "Position" };
            _positionRepositoryMock.Setup(repo => repo.GetPositionByIdAsync(positionId)).ReturnsAsync(position);

            // Act
            var result = await _positionService.GetPositionByIdAsync(positionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(position.Id, result.Id);
            Assert.Equal(position.PositionName, result.PositionName);
        }

        [Fact]
        public async Task AddPositionAsync_AddsNewPosition()
        {
            // Arrange
            var newPositionDto = new PositionDTO { PositionName = "New Position" };
            var newPosition = _mapper.Map<Position>(newPositionDto);

            // Act
            await _positionService.AddPositionAsync(newPositionDto);

            // Assert
            _positionRepositoryMock.Verify(repo => repo.AddPositionAsync(It.IsAny<Position>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePositionAsync_UpdatesPosition()
        {
            // Arrange
            var positionId = Guid.NewGuid();
            var updatedPositionDto = new PositionDTO { Id = positionId, PositionName = "Updated Position" };
            var existingPosition = new Position { Id = positionId, PositionName = "Original Position" };
            _positionRepositoryMock.Setup(repo => repo.GetPositionByIdAsync(positionId)).ReturnsAsync(existingPosition);

            // Act
            await _positionService.UpdatePositionAsync(positionId, updatedPositionDto);

            // Assert
            _positionRepositoryMock.Verify(repo => repo.UpdatePositionAsync(existingPosition), Times.Once);
            Assert.Equal(updatedPositionDto.PositionName, existingPosition.PositionName);
        }

        [Fact]
        public async Task DeletePositionAsync_DeletesPosition()
        {
            // Arrange
            var positionId = Guid.NewGuid();

            // Act
            await _positionService.DeletePositionAsync(positionId);

            // Assert
            _positionRepositoryMock.Verify(repo => repo.DeletePositionAsync(positionId), Times.Once);
        }
    }
}
