using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.Services.LoggingServices
{
    public interface ILoggingService
    {
        Task<IEnumerable<Log>> GetAllLogs();
        Task<Log> GetLogById(Guid id);
        Task CreateLog(Log log);
        Task UpdateLog(Log log);
        Task DeleteLog(Guid id);
        Task LogActionAsync(string action, string entity, string userName);

        Task<IEnumerable<Log>> GetLogsByMonthYear(int month, int year);
    }
}
