using Domain.Models;

namespace Persistence.Repositories
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<Feedback>> GetFeedbacksAsync();
        Task<Feedback> GetFeedbackByIdAsync(Guid Id);
        void Add(Feedback feedback);
        void Delete(Feedback feedback);
    }
}
