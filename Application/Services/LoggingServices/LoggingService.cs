using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.LoggingServices
{
    public class LoggingService : ILoggingService
    {
        private readonly DataContext _context;

        public LoggingService(DataContext context)
        {
            _context = context;
        }

        public async Task LogActionAsync(string action, string entity, string userName)
        {
            var log = new Log
            {
                Action = action,
                Entity = entity,
                UserName = userName,
                Timestamp = DateTime.UtcNow
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Log>> GetAllLogs()
        {
            return await _context.Logs.ToListAsync();
        }

        public async Task<Log> GetLogById(Guid id)
        {
            return await _context.Logs.FindAsync(id);
        }

        public async Task CreateLog(Log log)
        {
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLog(Log log)
        {
            _context.Entry(log).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLog(Guid id)
        {
            var log = await _context.Logs.FindAsync(id);
            if (log != null)
            {
                _context.Logs.Remove(log);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Log>> GetLogsByMonthYear(int month, int year)
        {
            return await _context.Logs.Where(log => log.Timestamp.Month == month && log.Timestamp.Year == year).ToListAsync();
        }

    }
}
