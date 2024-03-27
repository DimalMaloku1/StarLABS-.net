using Domain.Models;

namespace Domain.Contracts
{
    public interface ILoggingRepository
    {
        Task<IEnumerable<Log>> GetAllLogsAsync();
        Task<Log> GetLogByIdAsync(Guid id);
        Task CreateLogAsync(Log log);
        Task UpdateLogAsync(Log log);
        Task DeleteLogAsync(Guid id);
        Task<IEnumerable<Log>> GetLogsByMonthYearAsync(int month, int year);
        Task LogActionAsync(string action, string entity, string userName);
    }
}
