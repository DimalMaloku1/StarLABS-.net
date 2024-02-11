using Application.DTOs;
using AutoMapper;
using Domain.Models;
using Persistence.Repositories;

namespace Application.Services.BookingServices
{
    internal sealed class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetBookingsAsync();
            var bookingsDto = _mapper.Map<IEnumerable<BookingDto>>(bookings);
            return bookingsDto;
        }

        public async Task<BookingDto> GetBookingByIdAsync(Guid id)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            var bookingDto = _mapper.Map<BookingDto>(booking);
            return bookingDto;
        }
        public async Task<BookingDto> CreateAsync(BookingDto bookingDto)
        {
            var booking = _mapper.Map<Booking>(bookingDto);
            await _bookingRepository.Add(booking);
            return bookingDto;
        }
        //TODO: complete the functionality to save the mappedentity to the database 
        public async Task UpdateAsync(Guid id, BookingDto bookingDto)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);

            _mapper.Map(bookingDto, booking);

            await _bookingRepository.Update(booking);

        }
        public async Task DeleteAsync(Guid id)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            await _bookingRepository.Delete(booking);

        }

    }
}
