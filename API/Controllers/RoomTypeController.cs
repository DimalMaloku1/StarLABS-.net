using System;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Services.RoomServices;
using Application.Services.RoomTypeServices;
using Application.Validations;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RoomTypeController : Controller
    {
        private readonly IRoomTypeServices _roomTypeService;
        private readonly IRoomServices _roomService;

        private readonly RoomTypeValidator _roomTypeValidator;
        public RoomTypeController(IRoomTypeServices roomTypeService, RoomTypeValidator roomTypeValidator, IRoomServices roomService)
        {
            _roomTypeService = roomTypeService;
            _roomTypeValidator = roomTypeValidator;
            _roomService = roomService;
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
            var validationResult = _roomTypeValidator.Validate(roomTypeDto);

            if (validationResult.IsValid)
            {
                var createdRoomType = await _roomTypeService.CreateAsync(roomTypeDto);
                return RedirectToAction(nameof(Index));
            }
            return View(roomTypeDto);
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
            var validationResult = _roomTypeValidator.Validate(roomTypeDto);

            if (validationResult.IsValid)
            {
                await _roomTypeService.UpdateAsync(Id, roomTypeDto);
                return RedirectToAction(nameof(Details), new { Id });
            }

            return View(roomTypeDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            await _roomTypeService.DeleteAsync(Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
