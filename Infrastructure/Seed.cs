using Microsoft.AspNetCore.Identity;
using Domain.Models;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class Seed
    {
        public static async Task SeedData(RoleManager<IdentityRole<Guid>> _roleManager, DataContext _context)
        {
            // Ensure Admin and User roles exist
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            }
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>("User"));
            }

            await _context.SaveChangesAsync();

            // Seed RoomTypes if not already present
            var roomTypes = new List<RoomType>
            {
                new RoomType { Type = "Standard", Description = "Standard Room", Price = 100, Capacity = 2 },
                new RoomType { Type = "Suite", Description = "Suite Room", Price = 200, Capacity = 4 },
                new RoomType { Type = "Deluxe", Description = "Deluxe Room", Price = 300, Capacity = 6 }
            };

            foreach (var roomType in roomTypes)
            {
                if (!_context.RoomTypes.Any(rt => rt.Type == roomType.Type))
                {
                    await _context.RoomTypes.AddAsync(roomType);
                }
            }
            await _context.SaveChangesAsync();

            // Seed Rooms if not already present
            var existingRooms = _context.Rooms.Select(r => r.RoomTypeId).ToList();

            var roomsToAdd = new List<Room>();
            var random = new Random();

            foreach (var roomType in roomTypes)
            {
                if (!existingRooms.Contains(roomType.Id))
                {
                    for (int i = 1; i <= 1; i++) // Creating 10 rooms
                    {
                        var room = new Room
                        {
                            RoomNumber = i,
                            IsFree = true,
                            RoomTypeId = roomType.Id
                        };
                        roomsToAdd.Add(room);
                    }
                }
            }

            if (roomsToAdd.Any())
            {
                await _context.Rooms.AddRangeAsync(roomsToAdd);
                await _context.SaveChangesAsync();
            }
        }
    }
}
