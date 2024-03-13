using Application.DTOs;
using System;

namespace Application.Services.BookingServices
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
        Task<BookingDto> GetBookingByIdAsync(Guid id);
        Task<BookingDto> CreateAsync(BookingDto booking, Guid userId, string baseUrl);
        Task UpdateAsync(Guid id, BookingDto bookingDto);
        Task DeleteAsync(Guid id);
        Task<NewBookingDropDownsDTO> GetNewBookingDropDownsValues();
    }
}
