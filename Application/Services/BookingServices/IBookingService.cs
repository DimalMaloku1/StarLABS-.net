using Application.DTOs;

namespace Application.Services.BookingServices
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
        Task<BookingDto> GetBookingByIdAsync(Guid id);
        Task<BookingDto> CreateAsync(BookingDto booking);
        Task UpdateAsync(Guid id, BookingDto bookingDto);
        Task DeleteAsync(Guid id);
    }
}
