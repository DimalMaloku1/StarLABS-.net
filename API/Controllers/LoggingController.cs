using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Services.LoggingServices;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;




namespace API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LoggingController : Controller
    {
        private readonly ILoggingService _loggingService;

        public LoggingController(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        // GET: /Log
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var logs = await _loggingService.GetAllLogs();
            return View(logs);
        }

        // GET: /Log/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var log = await _loggingService.GetLogById(id);
            if (log == null)
            {
                return NotFound();
            }
            return View(log);
        }

        // GET: /Log/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Log/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Log log)
        {
            if (ModelState.IsValid)
            {
                await _loggingService.CreateLog(log);
                return RedirectToAction(nameof(Index));
            }
            return View(log);
        }

        // GET: /Log/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var log = await _loggingService.GetLogById(id);
            if (log == null)
            {
                return NotFound();
            }
            return View(log);
        }

        // POST: /Log/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Log log)
        {
            if (id != log.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _loggingService.UpdateLog(log);
                return RedirectToAction(nameof(Index));
            }
            return View(log);
        }

        // GET: /Log/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var log = await _loggingService.GetLogById(id);
            if (log == null)
            {
                return NotFound();
            }
            return View(log);
        }

        // POST: /Log/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _loggingService.DeleteLog(id);
            return RedirectToAction(nameof(Index));
        }



        // Inside LoggingController

        // GET: /Logging/DeleteLogs
        [HttpGet]
        public async Task<IActionResult> DeleteLogs(string monthYear)
        {
            if (DateTime.TryParse(monthYear, out var selectedMonthYear))
            {
                // Get logs for the specified month and year
                var logsToDelete = await _loggingService.GetLogsByMonthYear(selectedMonthYear.Month, selectedMonthYear.Year);

                // Delete each log
                foreach (var log in logsToDelete)
                {
                    await _loggingService.DeleteLog(log.Id);
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Handle invalid input
                return BadRequest();
            }
        }


    }
}
