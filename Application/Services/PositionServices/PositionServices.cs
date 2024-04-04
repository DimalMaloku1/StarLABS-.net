using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;

namespace Application.Services.PositionServices
{
    public class PositionServices(IPositionRepository positionRepository, IMapper mapper) : IPositionServices
    {
        private readonly IPositionRepository _positionRepository = positionRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<PositionDTO>> GetAllPositionsAsync()
        {
            var positions = await _positionRepository.GetAllPositionsAsync();
            return _mapper.Map<IEnumerable<PositionDTO>>(positions);
        }

        public async Task<PositionDTO> GetPositionByIdAsync(Guid id)
        {
            var position = await _positionRepository.GetPositionByIdAsync(id);
            return _mapper.Map<PositionDTO>(position);
        }

        public async Task AddPositionAsync(PositionDTO positionDto)
        {
            var position = _mapper.Map<Position>(positionDto);
            await _positionRepository.AddPositionAsync(position);
        }

        public async Task UpdatePositionAsync(Guid id, PositionDTO positionDto)
        {
            var position = await _positionRepository.GetPositionByIdAsync(id);

            if (position != null)
            {
                _mapper.Map(positionDto, position);
                await _positionRepository.UpdatePositionAsync(position);
            }
            else
            {
                throw new ArgumentException($"Position with ID {id} not found.");
            }
        }

        public async Task DeletePositionAsync(Guid id)
        {
            await _positionRepository.DeletePositionAsync(id);
        }
    }
}
