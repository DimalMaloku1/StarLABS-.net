using Application.DTOs;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.FeedbackServices
{
    internal sealed class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public FeedbackService(IFeedbackRepository feedbackRepository, IMapper mapper, UserManager<AppUser> userManager)
        {
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<FeedbackDto>> GetAllFeedbacksAsync()
        {
            var feedbacks = await _feedbackRepository.GetFeedbacksAsync();
            var feedbacksDto = _mapper.Map<IEnumerable<FeedbackDto>>(feedbacks);
            return feedbacksDto;
        }

        public async Task<FeedbackDto> GetFeedbackByIdAsync(Guid id)
        {
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(id);
            var feedbackDto = _mapper.Map<FeedbackDto>(feedback);
            return feedbackDto;
        }
        public async Task<FeedbackDto> CreateAsync(FeedbackDto feedbackDto)
        {
            var feedback = new Feedback()
            {
                Comment = feedbackDto.Comment,
                Rating = feedbackDto.Rating,
                UserId = feedbackDto.UserId
            };
            await _feedbackRepository.Add(feedback);
            return feedbackDto;
        }
        public async Task UpdateAsync(Guid id, FeedbackDto feedbackDto)
        {
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(id);

            if (feedback == null)
            {

                return;
            }

            feedback.Comment = feedbackDto.Comment;
            feedback.Rating = feedbackDto.Rating;


            await _feedbackRepository.Update(feedback);
        }


        public async Task DeleteAsync(Guid id)
        {
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(id);
            await _feedbackRepository.Delete(feedback);
        }

        public async Task<double> CalculateAverageRatingAsync()
        {
            
            var feedbacks = await _feedbackRepository.GetFeedbacksAsync();

            
            if (feedbacks == null || !feedbacks.Any())
            {
                
                return 0;
            }

            
            int totalRatingSum = feedbacks.Sum(f => f.Rating);

           
            double averageRating = (double)totalRatingSum / feedbacks.Count();

            return averageRating;
        }

        public async Task<IEnumerable<FeedbackDto>> GetTopRatedFeedbacksAsync(int count)
        {
            var topRatedFeedbacks = await _feedbackRepository.GetFeedbacksAsync();
            var topRatedFeedbacksDto = topRatedFeedbacks
                .OrderByDescending(f => f.Rating)
                .Take(count)
                .Select(f => new FeedbackDto
                {
                    Id = f.Id, // Assuming Id is a property in FeedbackDto
                    Comment = f.Comment,
                    Rating = f.Rating,
                    // Map other properties as needed
                });

            return topRatedFeedbacksDto;
        }
    }
}
