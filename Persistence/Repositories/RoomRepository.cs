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
    internal sealed class RoomRepository : IRoomRepository

    {
        private readonly DataContext _context;

        public RoomRepository(DataContext context)
        {
            _context = context;
        }
 

        public async Task<Room> GetRoomByIdAsync(Guid Id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(x => x.Id == Id);
            return room;
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync()
        {
            var rooms = await _context.Rooms.ToListAsync();
            return rooms;
        }

        public async Task Add(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Room room)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid Id, Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }


    }
}
