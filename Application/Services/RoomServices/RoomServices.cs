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

namespace Application.Services.RoomServices
{
    internal sealed class RoomServices : IRoomServices
    {

        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapp;

        public RoomServices(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapp = mapper;
        }


        public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
        {
            var rooms = await _roomRepository.GetRoomsAsync();
            var roomsDto = _mapp.Map<IEnumerable<RoomDto>>(rooms);
            return roomsDto;
        }

        public async Task<RoomDto> CreateAsync(RoomDto roomDto)

        {
            var room = _mapp.Map<Room>(roomDto);
            await _roomRepository.Add(room);
            return roomDto;
        }

        public async Task DeleteAsync(Guid id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            await _roomRepository.Delete(room);

        }


        public async Task<RoomDto> GetRoomByIdAsync(Guid id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            var roomDto = _mapp.Map<RoomDto>(room);
            return roomDto;
        }

        public async Task UpdateAsync(Guid id, RoomDto roomDto)
        {
            var existingRoom = await _roomRepository.GetRoomByIdAsync(id);

            _mapp.Map(roomDto, existingRoom);
            await _roomRepository.UpdateAsync(id, existingRoom);

        }

        public async Task<IEnumerable<RoomDto>> GetRoomsByFreeStatusAsync(bool isFree)
        {
            var rooms = await _roomRepository.GetRoomsByFreeStatusAsync(isFree);
            var roomsDto = _mapp.Map<IEnumerable<RoomDto>>(rooms);
            return roomsDto;
        }

        public async Task<IEnumerable<RoomDto>> GetRoomsByRoomTypeIdAsync(Guid roomTypeId, bool isFree)
        {
            var rooms = await _roomRepository.GetRoomsByRoomTypeIdAsync(roomTypeId);

            rooms = rooms.Where(room => room.IsFree);

            var roomDtos = _mapp.Map<IEnumerable<RoomDto>>(rooms);
            return roomDtos;
        }

    }
}
