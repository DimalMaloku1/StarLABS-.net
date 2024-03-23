using Application.DTOs;
using Domain.Models;
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
        Task<RoomDto> GetAvailableRoomAsync(Guid roomTypeId, DateTime checkInDate, DateTime checkOutDate);

    }
}
