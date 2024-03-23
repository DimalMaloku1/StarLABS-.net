using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure
{

    public class DataContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
            .HasOne(b => b.Room)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.RoomId)
            .OnDelete(DeleteBehavior.NoAction);
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
        public DbSet<Log> Logs { get; set; }
        public DbSet<RoomTypePhoto> RoomTypePhotos { get; set; }

        public DbSet<DailyTask> DailyTasks { get; set; }


        //TODO: Implement after changing the relations between room and booking
        //public DbSet<Booking_Room> Bookings_Rooms { get; set; }

    }
}