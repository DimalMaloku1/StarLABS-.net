using Application.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.PositionServices
{
    public interface IPositionServices
    {
        Task<IEnumerable<Position>> GetAllPositionsAsync();
        Task<Position> GetPositionByIdAsync(Guid id);
        Task AddPositionAsync(PositionDTO positionDto);
        Task UpdatePositionAsync(Guid id, PositionDTO positionDto);
        Task DeletePositionAsync(Guid id);
    }
}
