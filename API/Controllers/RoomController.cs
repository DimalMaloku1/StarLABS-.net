using Application.DTOs;
using Application.Services.RoomServices;
using Application.Services.RoomTypeServices;
using Application.Validations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;

namespace API.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomServices _roomServices;
        private readonly IRoomTypeServices _roomTypeService;
        private readonly RoomValidator _roomValidator;

        public RoomController(IRoomServices roomServices, IRoomTypeServices roomTypeService, RoomValidator roomValidator)
        {
            _roomServices = roomServices;
            _roomTypeService = roomTypeService;
            _roomValidator = roomValidator;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _roomServices.GetAllRoomsAsync();



          //  ViewBag.RoomTypes = await _roomTypeService.GetAllRoomTypesAsync();

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
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> GetRoomsByFreeStatus(bool isFree)
        {
            var rooms = await _roomServices.GetRoomsByFreeStatusAsync(isFree);
            foreach (var room in rooms)
            {
                var roomType = await _roomTypeService.GetRoomTypeByIdAsync(room.RoomTypeId);
                room.Type = roomType?.Type;
                room.Price = roomType?.Price ?? 0;
            }

            return View(nameof(Index), rooms);
        }

        public async Task<IActionResult> GetFreeRooms()
        {
            return await GetRoomsByFreeStatus(true);
        }

        public async Task<IActionResult> GetOccupiedRooms()
        {
            return await GetRoomsByFreeStatus(false);
        }

        public async Task<IActionResult> GetRoomsByRoomTypeId(Guid roomTypeId)
        {
            var rooms = await _roomServices.GetRoomsByRoomTypeIdAsync(roomTypeId, isFree: true);
            foreach (var room in rooms)
            {
                var roomType = await _roomTypeService.GetRoomTypeByIdAsync(room.RoomTypeId);
                room.Type = roomType?.Type;
                room.Price = roomType?.Price ?? 0;
            }

            ViewBag.RoomTypes = await _roomTypeService.GetAllRoomTypesAsync();

            return View("Index", rooms);
        }
    }
}