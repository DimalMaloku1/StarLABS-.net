using Application.DTOs;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;

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
        
        public async Task<RoomDto> GetAvailableRoomAsync(Guid roomTypeId, DateTime checkInDate, DateTime checkOutDate)
        {
            var rooms = await _roomRepository.GetRoomsByTypeAsync(roomTypeId);

            var availableRoom = rooms.FirstOrDefault(room =>
                !room.Bookings.Any(booking =>
                    (checkInDate >= booking.CheckInDate && checkInDate < booking.CheckOutDate) ||
                    (checkOutDate > booking.CheckInDate && checkOutDate <= booking.CheckOutDate)));

            var availableRoomDto = availableRoom != null ? new RoomDto
            {
                Id = availableRoom.Id,
                RoomNumber = availableRoom.RoomNumber,
                RoomTypeId = availableRoom.RoomTypeId,
            } : null;

            return availableRoomDto;
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

    }
}
