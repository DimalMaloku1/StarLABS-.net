using Application.DTOs;
using Application.Services.RoomServices;
using Application.Services.RoomTypeServices;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Persistence.Repositories;

namespace Application.Services.BookingServices
{
    internal sealed class BookingService(IRoomServices _roomService,IRoomTypeServices _roomTypeService,
        IBookingRepository _bookingRepository, IMapper _mapper): IBookingService
    {
        public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetBookingsAsync();
            var bookingsDto = _mapper.Map<IEnumerable<BookingDto>>(bookings);
            return bookingsDto;
        }

        public async Task<BookingDto> GetBookingByIdAsync(Guid id)
        {
            var booking = await GetBookingById(id);
            var bookingDto = _mapper.Map<BookingDto>(booking);
            return bookingDto;
        }
        public async Task<BookingDto> CreateAsync(BookingDto bookingDto, Guid userId)
        {
            var booking = _mapper.Map<Booking>(bookingDto);
            booking.UserId = userId; 
            await _bookingRepository.Add(booking);

            foreach (var roomId in bookingDto.RoomIds)
            {
                var newBookingRoom = new Booking_RoomDto()
                {
                    BookingId = bookingDto.Id,
                    RoomId = roomId,
                };
                //await _bookingRepository
            }



            return _mapper.Map<BookingDto>(booking);
        }
        public async Task UpdateAsync(Guid id, BookingDto bookingDto)
        {
            var booking = await GetBookingById(id);

            _mapper.Map(bookingDto, booking);

            await _bookingRepository.Update(booking);

        }
        public async Task DeleteAsync(Guid id)
        {
            var booking = await GetBookingById(id);
            await _bookingRepository.Delete(booking);

        }

        public async Task<NewBookingDropDownsDTO> GetNewBookingDropDownsValues()
        {
            var response = new NewBookingDropDownsDTO()
            {
                Rooms = await _roomService.GetAllRoomsAsync(),
                RoomTypes = await _roomTypeService.GetAllRoomTypesAsync()
            };

            return response;

        }

        private async Task<Booking> GetBookingById(Guid id)
        {
            return await _bookingRepository.GetBookingByIdAsync(id);
        }


    }
}
