using Domain.Models;

namespace Application.Services.LoggingServices
{
    public interface ILoggingService
    {
        Task<IEnumerable<Log>> GetAllLogsAsync();
        Task<Log> GetLogByIdAsync(Guid id);
        Task CreateLogAsync(Log log);
        Task UpdateLogAsync(Log log);
        Task DeleteLogAsync(Guid id);
        Task LogActionAsync(string action, string entity, string userName);
        Task<IEnumerable<Log>> GetLogsByMonthYearAsync(int month, int year);
    }
}
