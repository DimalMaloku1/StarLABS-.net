using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal sealed class StaffRepository : IStaffRepository
    {
        private readonly DataContext _dbContext;

        public StaffRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Staff> GetStaffByIdAsync(Guid id)
        {
            return await _dbContext.Staff.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddStaffAsync(Staff staff)
        {
            await _dbContext.Staff.AddAsync(staff);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateStaffAsync(Staff staff)
        {
            _dbContext.Staff.Update(staff);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteStaffAsync(Guid id)
        {
            var staff = await _dbContext.Staff.FindAsync(id);
            if (staff != null)
            {
                _dbContext.Staff.Remove(staff);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Staff>> GetAllStaffAsync()
        {
            return await _dbContext.Staff
                .Include(s => s.User) 
                .Include(s => s.Position) 
                .ToListAsync();
        }

        public async Task<IEnumerable<Staff>> GetStaffByDepartmentAsync(string department)
        {
            return await _dbContext.Staff.Where(s => s.Department == department)
                .Include(s => s.User)
                .Include(s => s.Position)
                .ToListAsync();
        }

        public async Task<IEnumerable<Staff>> GetStaffByPositionAsync(Guid positionId)
        {
            return await _dbContext.Staff.Where(s => s.PositionId == positionId)
                .Include(s => s.User)
                .Include(s => s.Position)
                .ToListAsync();
        }
    }
}
