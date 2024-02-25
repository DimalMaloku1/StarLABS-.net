using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.RoomServices
{
    public interface IRoomServices
    {
        Task<IEnumerable<RoomDto>> GetAllRoomsAsync();
        Task<RoomDto> GetRoomByIdAsync(Guid id);
        Task<RoomDto> CreateAsync(RoomDto room);
        Task UpdateAsync(Guid id, RoomDto roomDto);
        Task DeleteAsync(Guid id);

        Task<IEnumerable<RoomDto>> GetRoomsByFreeStatusAsync(bool isFree);

        Task<IEnumerable<RoomDto>> GetRoomsByRoomTypeIdAsync(Guid roomTypeId, bool isFree);

    }
}
