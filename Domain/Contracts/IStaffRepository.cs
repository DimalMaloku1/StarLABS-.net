using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IStaffRepository
    {
        Task<Staff> GetStaffByIdAsync(Guid id);

        Task AddStaffAsync(Staff staff);

        Task UpdateStaffAsync(Staff staff);

        Task DeleteStaffAsync(Guid id);

        Task<IEnumerable<Staff>> GetAllStaffAsync();

        Task<IEnumerable<Staff>> GetStaffByDepartmentAsync(string department);

        Task<IEnumerable<Staff>> GetStaffByPositionAsync(Guid positionId);

    }
}
