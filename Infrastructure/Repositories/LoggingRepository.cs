using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
