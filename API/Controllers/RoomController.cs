using Application.DTOs;
using Application.Services.LoggingServices;
using Application.Services.RoomServices;
using Application.Services.RoomTypeServices;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class RoomController(IRoomServices _roomServices, IRoomTypeServices _roomTypeService, IValidator<RoomDto> _roomValidator, ILoggingService _loggingService) : Controller
    {
        private async Task<RoomDto> ProcessRoomDataAsync(Guid id)
        {
            var roomDto = await _roomServices.GetRoomByIdAsync(id);
            if (roomDto != null)
            {
                var roomType = await _roomTypeService.GetRoomTypeByIdAsync(roomDto.RoomTypeId);
                if (roomType != null)
                {
                    roomDto.Type = roomType.Type;
                    roomDto.Description = roomType.Description;
                    roomDto.Price = roomType.Price;
                    roomDto.Capacity = roomType.Capacity;
                }
            }
            return roomDto;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _roomServices.GetAllRoomsAsync();
            foreach (var room in rooms)
            {
                var roomType = await _roomTypeService.GetRoomTypeByIdAsync(room.RoomTypeId);
                if (roomType != null)
                {
                    room.Type = roomType.Type;
                    room.Price = roomType.Price;
                }
            }
            return View(rooms);
        }

        public async Task<IActionResult> Create()
        {
            var roomTypes = await _roomTypeService.GetAllRoomTypesAsync();
            var roomDto = new RoomDto
            {
                RoomTypes = roomTypes
            };
            return View(roomDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomDto roomDto)
        {
            var validationResult = _roomValidator.Validate(roomDto);
            if (validationResult.IsValid)
            {
                await _roomServices.CreateAsync(roomDto);
                await _loggingService.LogActionAsync("Created", "Room", User.FindFirst(ClaimTypes.Email)?.Value);
                return RedirectToAction(nameof(Index));
            }
            roomDto.RoomTypes = await _roomTypeService.GetAllRoomTypesAsync();
            return View(roomDto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var roomDto = await ProcessRoomDataAsync(id);
            if (roomDto == null)
            {
                return NotFound();
            }
            roomDto.RoomTypes = await _roomTypeService.GetAllRoomTypesAsync();
            return View(roomDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, RoomDto roomDto)
        {
            var validationResult = _roomValidator.Validate(roomDto);
            if (id != roomDto.Id)
            {
                return NotFound();
            }
            if (validationResult.IsValid)
            {
                await _roomServices.UpdateAsync(id, roomDto);
                await _loggingService.LogActionAsync("Updated", "Room", User.FindFirst(ClaimTypes.Email)?.Value);
                return RedirectToAction(nameof(Index));
            }
            roomDto.RoomTypes = await _roomTypeService.GetAllRoomTypesAsync();
            return View(roomDto);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var roomDto = await ProcessRoomDataAsync(id);
            if (roomDto == null)
            {
                return NotFound();
            }
            return View(roomDto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _roomServices.DeleteAsync(id);
            await _loggingService.LogActionAsync("Deleted", "Room", User.FindFirst(ClaimTypes.Email)?.Value);
            return RedirectToAction(nameof(Index));
        }
    }
}
