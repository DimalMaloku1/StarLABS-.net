using Application.DTOs;
using Application.Services.DailyTaskServices;
using Application.Services.StaffServices;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class DailyTaskController(IDailyTaskService _dailyTaskService, IStaffService _staffService, UserManager<AppUser> _userManager) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dailyTasks = await _dailyTaskService.GetAllDailyTasksAsync();
            foreach (var task in dailyTasks)
            {
                var staffFullName = await _staffService.GetStaffFullNameByStaffIdAsync(task.StaffId);
                task.StaffFullName = staffFullName;
            }
            return View(dailyTasks);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var staffs = await _staffService.GetAllStaffAsync();
            var dailyTaskDto = new DailyTaskDto
            {
                Staffs = staffs.Select(s => new StaffDTO
                {
                    Id = s.Id,
                    Name = $"{s.User.UserName} {s.User.UserLastname}"
                })
            };
            return View(dailyTaskDto);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(DailyTaskDto dailyTaskDto)
        {
            if (ModelState.IsValid)
            {
                await _dailyTaskService.CreateAsync(dailyTaskDto);
                return RedirectToAction(nameof(Index));
            }
            dailyTaskDto.Staffs = await _staffService.GetAllStaffAsync();
            return View(dailyTaskDto);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var dailyTaskDto = await _dailyTaskService.GetDailyTaskByIdAsync(id);
            if (dailyTaskDto == null)
            {
                return NotFound();
            }
            var staffs = await _staffService.GetAllStaffAsync();
            dailyTaskDto.Staffs = staffs.Select(s => new StaffDTO
            {
                Id = s.Id,
                Name = $"{s.User.UserName} {s.User.UserLastname}"
            });
            return View(dailyTaskDto);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id, DailyTaskDto dailyTaskDto)
        {
            if (id != dailyTaskDto.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _dailyTaskService.UpdateAsync(id, dailyTaskDto);
                }
                catch (Exception)
                {
                    return BadRequest("Failed to update the daily task.");
                }
                return RedirectToAction(nameof(Index));
            }

            var staffs = await _staffService.GetAllStaffAsync();
            dailyTaskDto.Staffs = staffs.Select(s => new StaffDTO
            {
                Id = s.Id,
                Name = $"{s.User.UserName} {s.User.UserLastname}"
            });

            return View(dailyTaskDto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _dailyTaskService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("api/[controller]")]
        public async Task<ActionResult<IEnumerable<DailyTaskDto>>> GetAllDailyTasksAsync()
        {
            var dailyTasks = await _dailyTaskService.GetAllDailyTasksAsync();
            return Ok(dailyTasks);
        }

        [HttpGet("api/[controller]/{id}")]
        public async Task<ActionResult<DailyTaskDto>> GetDailyTaskByIdAsync(Guid id)
        {
            var dailyTask = await _dailyTaskService.GetDailyTaskByIdAsync(id);
            if (dailyTask == null)
            {
                return NotFound();
            }
            return Ok(dailyTask);
        }

        [HttpPost("FilterByStatus")]
        public async Task<IActionResult> FilterByStatus(string status)
        {
            var dailyTasks = await _dailyTaskService.GetAllDailyTasksAsync();
            foreach (var task in dailyTasks)
            {
                var staffFullName = await _staffService.GetStaffFullNameByStaffIdAsync(task.StaffId);
                task.StaffFullName = staffFullName;
            }
            var filteredTasks = dailyTasks.Where(task => task.Status == status);
            return View("Index", filteredTasks);
        }

        [HttpGet("AllDailyTasks")]
        public async Task<IActionResult> AllDailyTasks()
        {
            var dailyTasks = await _dailyTaskService.GetAllDailyTasksAsync();
            foreach (var task in dailyTasks)
            {
                var staffFullName = await _staffService.GetStaffFullNameByStaffIdAsync(task.StaffId);
                task.StaffFullName = staffFullName;
            }
            return View("Index", dailyTasks);
        }

        [HttpGet("FilterByDate")]
        public async Task<IActionResult> FilterByDate(DateTime date)
        {
            var dailyTasks = await _dailyTaskService.GetDailyTasksByDateAsync(date);
            foreach (var task in dailyTasks)
            {
                var staffFullName = await _staffService.GetStaffFullNameByStaffIdAsync(task.StaffId);
                task.StaffFullName = staffFullName;
            }
            return View("Index", dailyTasks);
        }

        [HttpGet("TodayTasks")]
        public async Task<IActionResult> TodayTasks()
        {
            var todayTasks = await _dailyTaskService.GetDailyTasksByDateAsync(DateTime.Today);
            foreach (var task in todayTasks)
            {
                var staffFullName = await _staffService.GetStaffFullNameByStaffIdAsync(task.StaffId);
                task.StaffFullName = staffFullName;
            }
            return View(todayTasks);
        }

        [HttpGet("MyDailyTasks")]
        public async Task<IActionResult> MyDailyTasks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("User ID not found.");
            }
            var dailyTasks = await _dailyTaskService.GetDailyTasksByUserIdAsync(Guid.Parse(userId));
            foreach (var task in dailyTasks)
            {
                var staffFullName = await _staffService.GetStaffFullNameByStaffIdAsync(task.StaffId);
                task.StaffFullName = staffFullName;
            }
            return View("MyDailyTasks", dailyTasks);
        }

        [HttpGet]
        public async Task<IActionResult> EditTask(Guid id)
        {
            var task = await _dailyTaskService.GetDailyTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            var staffs = await _staffService.GetAllStaffAsync();
            task.Staffs = staffs.Select(s => new StaffDTO
            {
                Id = s.Id,
                Name = $"{s.User.UserName} {s.User.UserLastname}"
            });
            return View("EditTask", task);
        }

        [HttpPost("EditPostTask")]
        public async Task<IActionResult> EditPostTask(Guid id, DailyTaskDto dailyTaskDto)
        {
            if (id != dailyTaskDto.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _dailyTaskService.UpdateAsync(id, dailyTaskDto);
                }
                catch (Exception)
                {
                    return BadRequest("Failed to update the daily task.");
                }
                return RedirectToAction(nameof(MyDailyTasks));
            }
            dailyTaskDto.Staffs = await _staffService.GetAllStaffAsync();

            return View("EditTask", dailyTaskDto);
        }
    }
}
