using Application.DTOs;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;

namespace Application.Services.FeedbackServices
{
    internal sealed class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IMapper _mapper;

        public FeedbackService(IFeedbackRepository feedbackRepository, IMapper mapper)
        {
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
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
            var feedback = _mapper.Map<Feedback>(feedbackDto);
            await _feedbackRepository.Add(feedback);
            return feedbackDto;
        }
        public async Task UpdateAsync(Guid id, FeedbackDto feedbackDto)
        {
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(id);

            _mapper.Map(feedbackDto, feedback);

            //await _feedbackRepository.Update(feedback);

        }
        public async Task DeleteAsync(Guid id)
        {
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(id);
            await _feedbackRepository.Delete(feedback);

        }

    }
}
