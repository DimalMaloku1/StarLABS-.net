using Domain.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Domain.Contracts;

namespace Infrastructure.Repositories
{
    //TODO: Implement after changing the relations between room and booking
    internal sealed class Booking_RoomRepository(DataContext _context): IBooking_RoomRepository
    {
        //public async Task<IEnumerable<Booking_Room>> GetBookingsAsync()
        //{
        //    var bookings_rooms = await _context.Bookings_Rooms
        //                        .Include(br => br.Booking)
        //                        .Include(br => br.Room)
        //                        .ToListAsync();
        //    return bookings_rooms;
        //}

        //public async Task<Booking_Room> GetBookingByIdAsync(Guid bookingId, Guid roomId)
        //{
        //    var booking_room = await _context.Bookings_Rooms
        //                        .Include(br => br.Booking)
        //                        .Include(br => br.Room)
        //                        .FirstOrDefaultAsync(x => x.BookingId == bookingId && x.RoomId == roomId);
        //    return booking_room;
        //}
        //public async Task Add(Booking_Room bookingRoom)
        //{
        //    await _context.Bookings_Rooms.AddAsync(bookingRoom);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task Update(Booking_Room bookingRoom)
        //{
        //    EntityEntry entityEntry = _context.Bookings_Rooms.Entry(bookingRoom);
        //    entityEntry.State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //}

        //public async Task Delete(Booking_Room bookingRoom)
        //{
        //    _context.Bookings_Rooms.Remove(bookingRoom);
        //    await _context.SaveChangesAsync();
        //}
    }
}
