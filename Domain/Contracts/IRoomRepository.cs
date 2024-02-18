using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetRoomsAsync();
        Task<Room> GetRoomByIdAsync(Guid Id);
        Task Add(Room room);
        Task Delete(Room room);

        Task UpdateAsync(Guid Id, Room room);
    }
}
