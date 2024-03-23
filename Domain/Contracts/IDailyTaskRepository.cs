using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IDailyTaskRepository
    {
        Task<IEnumerable<DailyTask>> GetDailyTasksAsync();
        Task<DailyTask> GetDailyTaskByIdAsync(Guid Id);
        Task Add(DailyTask dailyTask);
        Task Delete(DailyTask dailyTask);

        Task UpdateAsync(Guid Id, DailyTask dailyTask);
    }
}
