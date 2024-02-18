using Domain.Models;

namespace Domain.Contracts
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<Feedback>> GetFeedbacksAsync();
        Task<Feedback> GetFeedbackByIdAsync(Guid Id);
        Task Add(Feedback feedback);
        Task Delete(Feedback feedback);
    }
}
