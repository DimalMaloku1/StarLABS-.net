using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.StaffServices
{
    public interface IStaffService
    {
        Task<IEnumerable<StaffDTO>> GetAllStaffAsync();
        Task<StaffDTO> GetStaffByIdAsync(Guid id);
        Task AddStaffAsync(StaffDTO staffDTO);
        Task UpdateStaffAsync(Guid id, StaffDTO staffDTO);
        Task DeleteStaffAsync(Guid id);
        Task<IEnumerable<StaffDTO>> GetStaffByDepartmentAsync(string department);
        Task<IEnumerable<StaffDTO>> GetStaffByPositionAsync(Guid positionId);
        Task<IEnumerable<Guid>> GetAllUserNamesAsync();

    }
}
