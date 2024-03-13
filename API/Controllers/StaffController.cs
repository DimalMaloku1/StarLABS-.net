using Application.DTOs;
using Application.Services.PositionServices;
using Application.Services.StaffServices;
using Application.Validations;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Application.Services.LoggingServices;
using System.Security.Claims;

namespace MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StaffController : Controller
    {
        private readonly IStaffService _staffService;
        private readonly IPositionServices _positionService;
        private readonly IValidator<StaffDTO> _staffValidator;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILoggingService _loggingService;

        public StaffController(IStaffService staffService, IValidator<StaffDTO> staffValidator, IPositionServices positionServices, UserManager<AppUser> userManager, ILoggingService loggingService)
        {
            _staffService = staffService;
            _staffValidator = staffValidator;
            _positionService = positionServices;
            _userManager = userManager;
            _loggingService = loggingService;
        }

        public async Task<IActionResult> Index()
        {
            var staffList = await _staffService.GetAllStaffAsync();
            return View(staffList);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return View(staff);
        }

        public async Task<IActionResult> Create()
        {
            var users = await _userManager.Users.Select(u => new { u.Id, u.Email }).ToListAsync();
            ViewBag.Users = new SelectList(users, "Id", "Email");
            var positions = await _positionService.GetAllPositionsAsync();
            ViewBag.Positions = new SelectList(positions ,"Id", "PositionName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StaffDTO staffDTO)
        {
            var validationResult = _staffValidator.Validate(staffDTO);

            if (validationResult.IsValid)
            {
                await _staffService.AddStaffAsync(staffDTO);
                await _loggingService.LogActionAsync("Created", "Staff", User.FindFirst(ClaimTypes.Email)?.Value);
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            var users = await _userManager.Users.Select(u => new { u.Id, u.Email }).ToListAsync();
            ViewBag.Users = new SelectList(users, "Id", "Email");

            var positions = await _positionService.GetAllPositionsAsync();
            ViewBag.Positions = new SelectList(positions, "Id", "PositionName");
            return View(staffDTO);
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            var users = await _userManager.Users.Select(u => new { u.Id, u.Email }).ToListAsync();
            ViewBag.Users = new SelectList(users, "Id", "Email");

            var positions = await _positionService.GetAllPositionsAsync();
            ViewBag.Positions = new SelectList(positions, "Id", "PositionName");

            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StaffDTO staffDTO)
        {
            var validationResult = _staffValidator.Validate(staffDTO);

            if (id != staffDTO.Id)
            {
                return BadRequest();
            }

            if (validationResult.IsValid)
            {
                await _staffService.UpdateStaffAsync(id, staffDTO);
                await _loggingService.LogActionAsync("Edit", "Staff", User.FindFirst(ClaimTypes.Email)?.Value);
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            var users = await _userManager.Users.Select(u => new { u.Id, u.Email }).ToListAsync();
            ViewBag.Users = new SelectList(users, "Id", "Email");

            var positions = await _positionService.GetAllPositionsAsync();
            ViewBag.Positions = new SelectList(positions, "Id", "PositionName");

            return View(staffDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Department(string department)
        {
            if (string.IsNullOrEmpty(department))
            {
                return BadRequest();
            }

            var staff = await _staffService.GetStaffByDepartmentAsync(department);
            return View("StaffListByDepartment", staff);
        }
        [HttpPost]
        public async Task<IActionResult> Position(Guid positionId)
        {
            if (positionId == Guid.Empty)
            {
                return BadRequest("Position ID is required.");
            }

            var staff = await _staffService.GetStaffByPositionAsync(positionId);
            return View("StaffListByPosition", staff);
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return View(staff);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _staffService.DeleteStaffAsync(id);
            await _loggingService.LogActionAsync("Deleted", "Staff", User.FindFirst(ClaimTypes.Email)?.Value);
            return RedirectToAction(nameof(Index));
        }
    }
}
