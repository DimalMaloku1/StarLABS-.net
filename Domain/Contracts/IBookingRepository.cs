using Domain.Models;

namespace Persistence.Repositories
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetBookingsAsync();
        Task<Booking> GetBookingByIdAsync(Guid Id);
        Task Add(Booking booking);
        Task Update(Booking booking);
        Task Delete(Booking booking);
    }
}
