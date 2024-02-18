using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IRoomTypeRepository
    {
        Task<IEnumerable<RoomType>> GetRoomTypesAsync();
        Task<RoomType> GetRoomTypeByIdAsync(Guid Id);
        Task Add(RoomType roomType);
        Task Delete(RoomType roomType);

        Task UpdateAsync(Guid Id, RoomType roomType);
    }
}
