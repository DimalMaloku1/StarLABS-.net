using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Contracts
{
    public interface ILoggingRepository
    {
        Task<IEnumerable<Log>> GetAllLogs();
        Task<Log> GetLogById(Guid id);
        Task CreateLog(Log log);
        Task UpdateLog(Log log);
        Task DeleteLog(Guid id);
    }
}
