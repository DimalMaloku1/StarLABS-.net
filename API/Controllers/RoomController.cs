using Application.DTOs;
using Application.Services.RoomServices;
using Application.Services.RoomTypeServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomServices _roomServices;
        private readonly IRoomTypeServices _roomTypeService;

        public RoomController(IRoomServices roomServices, IRoomTypeServices roomTypeService)
        {
            _roomServices = roomServices;
            _roomTypeService = roomTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _roomServices.GetAllRoomsAsync();
            foreach (var room in rooms)
            {
                var roomType = await _roomTypeService.GetRoomTypeByIdAsync(room.RoomTypeId);
                room.Type = roomType?.Type; // Nëse roomType është null, atributi Type do të jetë null gjithashtu
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
            if (ModelState.IsValid)
            {
                await _roomServices.CreateAsync(roomDto);
                return RedirectToAction(nameof(Index));
            }

            // If model state is not valid, reload room types and return to the view
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
            if (id != roomDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _roomServices.UpdateAsync(id, roomDto);
                return RedirectToAction(nameof(Index));
            }

            // If model state is not valid, reload room types and return to the view
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
            return View(roomDto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _roomServices.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}