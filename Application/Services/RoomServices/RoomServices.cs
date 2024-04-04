using Application.DTOs;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.RoomServices
{
    public class RoomServices(IRoomRepository _roomRepository, IMapper _mapper) : IRoomServices
    {
        public async Task<RoomDto> GetAvailableRoomAsync(Guid roomTypeId, DateTime checkInDate, DateTime checkOutDate)
        {
            var rooms = await _roomRepository.GetRoomsByTypeAsync(roomTypeId);
            var availableRoom = FindAvailableRoom(rooms, checkInDate, checkOutDate);
            return availableRoom != null ? _mapper.Map<RoomDto>(availableRoom) : null;
        }
        private Room FindAvailableRoom(IEnumerable<Room> rooms, DateTime checkInDate, DateTime checkOutDate)
        {
            return rooms.FirstOrDefault(room =>
                !room.Bookings.Any(booking =>
                    (checkInDate >= booking.CheckInDate && checkInDate < booking.CheckOutDate) ||
                    (checkOutDate > booking.CheckInDate && checkOutDate <= booking.CheckOutDate)));
        }

        public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
        {
            var rooms = await _roomRepository.GetRoomsAsync();
            return _mapper.Map<IEnumerable<RoomDto>>(rooms);
        }

        public async Task<RoomDto> CreateAsync(RoomDto roomDto)
        {
            var room = _mapper.Map<Room>(roomDto);
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
            return _mapper.Map<RoomDto>(room);
        }

        public async Task UpdateAsync(Guid id, RoomDto roomDto)
        {
            var existingRoom = await _roomRepository.GetRoomByIdAsync(id);
            _mapper.Map(roomDto, existingRoom);
            await _roomRepository.UpdateAsync(id, existingRoom);
        }
    }
}
