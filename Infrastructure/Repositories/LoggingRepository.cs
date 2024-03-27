using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class LoggingRepository : ILoggingRepository
    {
        private readonly DataContext _context;

        public LoggingRepository(DataContext context)
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
        public async Task<IEnumerable<Log>> GetAllLogsAsync()
        {
            return await _context.Logs.ToListAsync();
        }

        public async Task<Log> GetLogByIdAsync(Guid id)
        {
            return await _context.Logs.FindAsync(id);
        }

        public async Task CreateLogAsync(Log log)
        {
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLogAsync(Log log)
        {
            _context.Entry(log).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLogAsync(Guid id)
        {
            var log = await _context.Logs.FindAsync(id);
            if (log != null)
            {
                _context.Logs.Remove(log);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Log>> GetLogsByMonthYearAsync(int month, int year)
        {
            return await _context.Logs.Where(log => log.Timestamp.Month == month && log.Timestamp.Year == year).ToListAsync();
        }
    }
}
