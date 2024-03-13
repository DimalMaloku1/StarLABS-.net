using System;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Services.RoomServices;
using Application.Services.RoomTypeServices;
using Application.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Application.Services.LoggingServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    public class RoomTypeController : Controller
    {
        private readonly IRoomTypeServices _roomTypeService;
        private readonly IRoomServices _roomService;
        private readonly  IValidator<RoomTypeDto> _roomTypeValidator;
        private readonly ILoggingService _loggingService;

        public RoomTypeController(IRoomTypeServices roomTypeService, IValidator<RoomTypeDto> roomTypeValidator, IRoomServices roomService, ILoggingService loggingService)
        {
            _roomTypeService = roomTypeService;
            _roomTypeValidator = roomTypeValidator;
            _roomService = roomService;
            _loggingService = loggingService;
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
        public async Task<IActionResult> Create(RoomTypeDto roomTypeDto, List<IFormFile> photos)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createdRoomType = await _roomTypeService.CreateAsync(roomTypeDto, photos);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Failed to create room type.");
                    return View(roomTypeDto);
                }
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
               // await _roomTypeService.UpdateAsync(Id, roomTypeDto);
                await _loggingService.LogActionAsync("Updated", "Room Type", User.FindFirst(ClaimTypes.Email)?.Value);
                return RedirectToAction(nameof(Details), new { Id });
            }

            return View(roomTypeDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            await _roomTypeService.DeleteAsync(Id);
            await _loggingService.LogActionAsync("Deleted", "Room Type", User.FindFirst(ClaimTypes.Email)?.Value);
            return RedirectToAction(nameof(Index));
        }
    }
}
