using Application.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.DailyTaskServices
{
    public interface IDailyTaskService
    {
        Task<IEnumerable<DailyTaskDto>> GetAllDailyTasksAsync();
        Task<DailyTaskDto> GetDailyTaskByIdAsync(Guid id);
        Task<DailyTaskDto> CreateAsync(DailyTaskDto dailyTaskDto);
        Task UpdateAsync(Guid id, DailyTaskDto dailyTaskDto);
        Task DeleteAsync(Guid id);

        Task<IEnumerable<DailyTaskDto>> GetDailyTasksByStatusAsync(string status);

        Task<IEnumerable<DailyTaskDto>> GetDailyTasksByDateAsync(DateTime date);

        Task<IEnumerable<DailyTaskDto>> GetDailyTasksByStaffIdAsync(Guid staffId);

        Task<IEnumerable<DailyTaskDto>> GetDailyTasksByUserIdAsync(Guid userId);


    }
}
