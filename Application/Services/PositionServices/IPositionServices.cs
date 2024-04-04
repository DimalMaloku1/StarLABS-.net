using Application.DTOs;

namespace Application.Services.PositionServices
{
    public interface IPositionServices
    {
        Task<IEnumerable<PositionDTO>> GetAllPositionsAsync();
        Task<PositionDTO> GetPositionByIdAsync(Guid id);
        Task AddPositionAsync(PositionDTO positionDto);
        Task UpdatePositionAsync(Guid id, PositionDTO positionDto);
        Task DeletePositionAsync(Guid id);
    }
}
