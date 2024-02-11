using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal sealed class FeedbackRepository : IFeedbackRepository
    {
        public Task<IEnumerable<Feedback>> GetFeedbacksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Feedback> GetFeedbackByIdAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public void Add(Feedback feedback)
        {
            throw new NotImplementedException();
        }

        public void Delete(Feedback feedback)
        {
            throw new NotImplementedException();
        }
    }
}
