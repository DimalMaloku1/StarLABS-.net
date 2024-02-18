using Application.DTOs;

namespace Application.Services.FeedbackServices
{
    public interface IFeedbackService
    {
        Task<IEnumerable<FeedbackDto>> GetAllFeedbacksAsync();
        Task<FeedbackDto> GetFeedbackByIdAsync(Guid id);
        Task<FeedbackDto> CreateAsync(FeedbackDto feedback);
        Task UpdateAsync(Guid id, FeedbackDto feedback);
        Task DeleteAsync(Guid id);
    }
}
