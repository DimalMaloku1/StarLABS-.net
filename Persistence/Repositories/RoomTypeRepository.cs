using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace Persistence.Repositories
{
    internal sealed class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly DataContext _context;

        public RoomTypeRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<RoomType> GetRoomTypeByIdAsync(Guid Id)
        {
            var roomType = await _context.RoomTypes.FirstOrDefaultAsync(x => x.Id == Id);
            return roomType;
        }

        public async Task<IEnumerable<RoomType>> GetRoomTypesAsync()
        {
            var roomTypes = await _context.RoomTypes.ToListAsync();
            return roomTypes;
        }

        public async Task Add(RoomType roomType)
        {
            await _context.RoomTypes.AddAsync(roomType);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(RoomType roomType)
        {
            _context.RoomTypes.Remove(roomType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid Id, RoomType roomType)
        {
            _context.RoomTypes.Update(roomType);
            await _context.SaveChangesAsync();
        }
    }
}
