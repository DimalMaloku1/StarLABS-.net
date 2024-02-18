using Application.DTOs;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.RoomTypeServices
{
    internal sealed class RoomTypeServices : IRoomTypeServices
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IMapper _mapp;

        public RoomTypeServices(IRoomTypeRepository roomTypeRepository, IMapper mapper)
        {
            _roomTypeRepository = roomTypeRepository;
            _mapp = mapper;
        }
        public async Task<RoomTypeDto> CreateAsync(RoomTypeDto roomTypeDto)
        {
            var roomType = _mapp.Map<RoomType>(roomTypeDto);
            await _roomTypeRepository.Add(roomType);
            return roomTypeDto;
        }

        public async Task DeleteAsync(Guid Id)
        {
            var roomType = await _roomTypeRepository.GetRoomTypeByIdAsync(Id);
            await _roomTypeRepository.Delete(roomType);
        }

        public async Task<IEnumerable<RoomTypeDto>> GetAllRoomTypesAsync()
        {
            var roomTypes = await _roomTypeRepository.GetRoomTypesAsync();
            var roomTypesDto = _mapp.Map<IEnumerable<RoomTypeDto>>(roomTypes);
            return roomTypesDto;
        }

        public async Task<RoomTypeDto> GetRoomTypeByIdAsync(Guid Id)
        {
            var roomType = await _roomTypeRepository.GetRoomTypeByIdAsync(Id);
            var roomTypeDto = _mapp.Map<RoomTypeDto>(roomType);
            return roomTypeDto;
        }

        public async Task UpdateAsync(Guid Id, RoomTypeDto roomTypeDto)
        {
            var existingRoomType = await _roomTypeRepository.GetRoomTypeByIdAsync(Id);

            _mapp.Map(roomTypeDto, existingRoomType);
            await _roomTypeRepository.UpdateAsync(Id, existingRoomType);
        }
    }
}
