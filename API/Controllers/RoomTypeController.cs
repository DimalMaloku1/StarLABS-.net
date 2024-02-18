using System;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Services.RoomTypeServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RoomTypeController : Controller
    {
        private readonly IRoomTypeServices _roomTypeService;

        public RoomTypeController(IRoomTypeServices roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roomTypes = await _roomTypeService.GetAllRoomTypesAsync();
            return View(roomTypes);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            var roomType = await _roomTypeService.GetRoomTypeByIdAsync(Id);
            if (roomType == null)
            {
                return NotFound();
            }
            return View(roomType);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoomTypeDto roomTypeDto)
        {
            var createdRoomType = await _roomTypeService.CreateAsync(roomTypeDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var roomType = await _roomTypeService.GetRoomTypeByIdAsync(Id);
            if (roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid Id, RoomTypeDto roomTypeDto)
        {
            await _roomTypeService.UpdateAsync(Id, roomTypeDto);
            return RedirectToAction(nameof(Details), new { Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            await _roomTypeService.DeleteAsync(Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
