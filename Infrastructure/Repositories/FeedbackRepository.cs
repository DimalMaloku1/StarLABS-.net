using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Repositories
{
    internal sealed class FeedbackRepository : IFeedbackRepository
    {
        private readonly DataContext _context;

        public FeedbackRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksAsync()
        {
            var feedbacks = await _context.Feedbacks.Include(x => x.User).ToListAsync();
            return feedbacks;
        }

        public async Task<Feedback> GetFeedbackByIdAsync(Guid Id)
        {
            var feedback = await _context.Feedbacks.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == Id);
            return feedback;
        }

        public async Task Add(Feedback feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Feedback feedback)
        {
            EntityEntry entityEntry = _context.Feedbacks.Entry(feedback);
            entityEntry.State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(Feedback feedback)
        {
            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
        }
    }
}
