using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Persistence.Repositories
{
    internal sealed class BookingRepository : IBookingRepository
    {
        private readonly DataContext _context;

        public BookingRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Booking>> GetBookingsAsync()
        {
            var bookings = await _context.Bookings.ToListAsync();
            return bookings;
        }


        public async Task<Booking> GetBookingByIdAsync(Guid Id)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(x => x.Id == Id);
            return booking;
        }

        public async Task Add(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Booking booking)
        {
            EntityEntry entityEntry = _context.Bookings.Entry(booking);
            entityEntry.State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(Booking booking)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }

    }
}
