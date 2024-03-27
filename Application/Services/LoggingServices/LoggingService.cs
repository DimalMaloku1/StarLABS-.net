using Domain.Contracts;
using Domain.Models;

namespace Application.Services.LoggingServices
{
    public class LoggingService : ILoggingService
    {
        private readonly ILoggingRepository _loggingRepository;

        public LoggingService(ILoggingRepository loggingRepository)
        {
            _loggingRepository = loggingRepository;
        }

        public async Task LogActionAsync(string action, string entity, string userName)
        {
            await _loggingRepository.LogActionAsync(action, entity, userName);
        }

        public async Task<IEnumerable<Log>> GetAllLogsAsync()
        {
            return await _loggingRepository.GetAllLogsAsync();
        }

        public async Task<Log> GetLogByIdAsync(Guid id)
        {
            return await _loggingRepository.GetLogByIdAsync(id);
        }

        public async Task CreateLogAsync(Log log)
        {
            await _loggingRepository.CreateLogAsync(log);
        }

        public async Task UpdateLogAsync(Log log)
        {
            await _loggingRepository.UpdateLogAsync(log);
        }

        public async Task DeleteLogAsync(Guid id)
        {
            await _loggingRepository.DeleteLogAsync(id);
        }

        public async Task<IEnumerable<Log>> GetLogsByMonthYearAsync(int month, int year)
        {
            return await _loggingRepository.GetLogsByMonthYearAsync(month, year);
        }
    }
}
