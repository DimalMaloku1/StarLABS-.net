using Application.DTOs;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.PositionServices
{
    internal sealed class PositionServices : IPositionServices
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IMapper _mapper;

        public PositionServices(IPositionRepository positionRepository, IMapper mapper)
        {
            _positionRepository = positionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Position>> GetAllPositionsAsync()
        {
            var positions = await _positionRepository.GetAllPositionsAsync();
            return positions;
        }

        public async Task<Position> GetPositionByIdAsync(Guid id)
        {
            var position = await _positionRepository.GetPositionByIdAsync(id);
            return position;
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
                // Handle the case where the position with the given ID is not found
                throw new ArgumentException($"Position with ID {id} not found.");
            }
        }

        public async Task DeletePositionAsync(Guid id)
        {
            await _positionRepository.DeletePositionAsync(id);
        }
    }
}
