﻿using Application.DTOs;
using Application.Services.StaffServices;
using Application.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class StaffController : Controller
    {
        private readonly IStaffService _staffService;
        private readonly StaffValidator _staffValidator; 

        public StaffController(IStaffService staffService, StaffValidator staffValidator)
        {
            _staffService = staffService;
            _staffValidator = staffValidator;
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
            var users = await _staffService.GetAllUserNamesAsync();
            ViewBag.Users = new SelectList(users);

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
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            var users = await _staffService.GetAllUserNamesAsync();
            ViewBag.Users = new SelectList(users);
            return View(staffDTO);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
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
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

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
            return RedirectToAction(nameof(Index));
        }
    }
}
