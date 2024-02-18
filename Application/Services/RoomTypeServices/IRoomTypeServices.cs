using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.RoomTypeServices
{
    public interface IRoomTypeServices
    {
        Task<IEnumerable<RoomTypeDto>> GetAllRoomTypesAsync();
        Task<RoomTypeDto> GetRoomTypeByIdAsync(Guid Id);
        Task<RoomTypeDto> CreateAsync(RoomTypeDto roomType);
        Task UpdateAsync(Guid Id, RoomTypeDto roomTypeDto);
        Task DeleteAsync(Guid Id);
    }
}
