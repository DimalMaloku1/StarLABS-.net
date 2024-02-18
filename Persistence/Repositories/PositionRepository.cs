using Domain.Contracts;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal class PositionRepository : IPositionRepository
    {
        public Task<List<Position>> GetAllPositionsAsync()
        {
            throw new NotImplementedException();
        }
        public Task<Position> GetPositionByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
        public Task AddPositionAsync(Position position)
        {
            throw new NotImplementedException();
        }
        public Task UpdatePositionAsync(Position position)
        {
            throw new NotImplementedException();
        }
        public Task DeletePositionAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

