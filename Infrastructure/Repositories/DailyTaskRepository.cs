
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal sealed class DailyTaskRepository : IDailyTaskRepository
    {
        private readonly DataContext _context;

        public DailyTaskRepository(DataContext context)
        {
            _context = context;
        }

        public async Task Add(DailyTask dailyTask)
        {
            _context.DailyTasks.Add(dailyTask);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(DailyTask dailyTask)
        {
            _context.DailyTasks.Remove(dailyTask);
            await _context.SaveChangesAsync();

        }

        public async Task<DailyTask> GetDailyTaskByIdAsync(Guid Id)
        {
            var dailyTask = await _context.DailyTasks.FirstOrDefaultAsync(x => x.Id == Id);
            return dailyTask;
        }

        public async Task<IEnumerable<DailyTask>> GetDailyTasksAsync()
        {
            var dailyTasks = await _context.DailyTasks
                 .Include(s => s.Staff)
                .ToListAsync();
            return dailyTasks;
        }

        public async Task UpdateAsync(Guid Id, DailyTask dailyTask)
        {
            _context.DailyTasks.Update(dailyTask);
            await _context.SaveChangesAsync();
        }
    }
}
