using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal sealed class PositionRepository : IPositionRepository
    {
        private readonly DataContext _dbContext;

        public PositionRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Position>> GetAllPositionsAsync()
        {
            return await _dbContext.Position.ToListAsync();
        }

        public async Task<Position> GetPositionByIdAsync(Guid id)
        {
            return await _dbContext.Position.FindAsync(id);
        }

        public async Task AddPositionAsync(Position position)
        {
            _dbContext.Position.Add(position);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePositionAsync(Position position)
        {
            _dbContext.Entry(position).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletePositionAsync(Guid id)
        {
            var position = await _dbContext.Position.FindAsync(id);
            if (position != null)
            {
                _dbContext.Position.Remove(position);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}

