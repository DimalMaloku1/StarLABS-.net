using Application.DTOs;
using Application.Services.RoomServices;
using Application.Services.RoomTypeServices;
using Application.Validations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Application.Services.LoggingServices; // Import the namespace for LoggingService
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        private readonly IRoomServices _roomServices;
        private readonly IRoomTypeServices _roomTypeService;
        private readonly IValidator<RoomDto> _roomValidator;
        private readonly ILoggingService _loggingService; 

        public RoomController(IRoomServices roomServices, IRoomTypeServices roomTypeService, IValidator<RoomDto> roomValidator, ILoggingService loggingService)
        {
            _roomServices = roomServices;
            _roomTypeService = roomTypeService;
            _roomValidator = roomValidator;
            _loggingService = loggingService; // Injected logging service
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _roomServices.GetAllRoomsAsync();

            foreach (var room in rooms)
            {
                var roomType = await _roomTypeService.GetRoomTypeByIdAsync(room.RoomTypeId);
                room.Type = roomType?.Type;
                room.Price = roomType?.Price ?? 0;
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
                await _loggingService.LogActionAsync("Created", "Room", User.FindFirst(ClaimTypes.Email)?.Value); // Log action
                return RedirectToAction(nameof(Index));
            }

            roomDto.RoomTypes = await _roomTypeService.GetAllRoomTypesAsync();
            return View(roomDto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var roomDto = await _roomServices.GetRoomByIdAsync(id);
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
                await _loggingService.LogActionAsync("Updated", "Room", User.FindFirst(ClaimTypes.Email)?.Value); // Log action
                return RedirectToAction(nameof(Index));
            }

            roomDto.RoomTypes = await _roomTypeService.GetAllRoomTypesAsync();
            return View(roomDto);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var roomDto = await _roomServices.GetRoomByIdAsync(id);
            if (roomDto == null)
            {
                return NotFound();
            }

            var roomType = await _roomTypeService.GetRoomTypeByIdAsync(roomDto.RoomTypeId);
            roomDto.Type = roomType?.Type;
            roomDto.Description = roomType?.Description;
            roomDto.Price = roomType?.Price ?? 0;
            roomDto.Capacity = roomType?.Capacity ?? 0;

            return View(roomDto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _roomServices.DeleteAsync(id);
            await _loggingService.LogActionAsync("Deleted", "Room", User.FindFirst(ClaimTypes.Email)?.Value); // Log action
            return RedirectToAction(nameof(Index));
        }

    }
}
