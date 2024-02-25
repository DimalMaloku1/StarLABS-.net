using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection.Emit;

namespace Persistence
{

    public class DataContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Bill> Bills { get; set; }
        //TODO: Implement after changing the relations between room and booking
        //public DbSet<Booking_Room> Bookings_Rooms { get; set; }

    }
}