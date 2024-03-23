using Application.Core;
using Application.DTOs;
using System;

namespace Application.Services.BookingServices
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
        Task<BookingDto> GetBookingByIdAsync(Guid id);
        Task<Result<BookingDto>> CreateAsync(BookingDto booking, Guid userId, string baseUrl);
        Task<Result<BookingDto>> UpdateAsync(Guid id, BookingDto bookingDto);
        Task DeleteAsync(Guid id);
        Task<NewBookingDropDownsDTO> GetNewBookingDropDownsValues();
    }
}
