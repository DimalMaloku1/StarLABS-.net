using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IPositionRepository
    {
        Task<List<Position>> GetAllPositionsAsync();
        Task<Position> GetPositionByIdAsync(Guid id);
        Task AddPositionAsync(Position position);
        Task UpdatePositionAsync(Position position);
        Task DeletePositionAsync(Guid id);
    }
}
