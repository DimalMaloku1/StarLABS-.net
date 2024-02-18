using Application.DTOs;
using Application.Services.StaffServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class StaffController : Controller
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StaffDTO staffDTO)
        {
            if (ModelState.IsValid)
            {
                await _staffService.AddStaffAsync(staffDTO);
                return RedirectToAction(nameof(Index));
            }
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
            if (id != staffDTO.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _staffService.UpdateStaffAsync(id, staffDTO);
                return RedirectToAction(nameof(Index));
            }
            return View(staffDTO);
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
