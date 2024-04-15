using Application.DTOs;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.EmailServices;
using Application.DTOs.AccountDTOs;

namespace Application.Services.DailyTaskServices
{
    public class DailyTaskService : IDailyTaskService
    {
        private readonly IDailyTaskRepository _dailyTaskRepository;
        private readonly IMapper _mapp;
        private readonly DataContext _context;
        private readonly IEmailService _emailService;
        public DailyTaskService(IDailyTaskRepository dailyTaskRepository, IMapper mapper, DataContext context, IEmailService emailService)
        {
            _dailyTaskRepository = dailyTaskRepository;
            _mapp = mapper;
            _context = context;
            _emailService = emailService;

        }
        public async Task<DailyTaskDto> CreateAsync(DailyTaskDto dailyTaskDto)
        {
            var dailyTask = _mapp.Map<DailyTask>(dailyTaskDto);
            await _dailyTaskRepository.Add(dailyTask);

            var staff = await _context.Staff
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == dailyTask.StaffId);

            if (staff != null && staff.User != null)
            {
                var userDto = _mapp.Map<UserDto>(staff.User);
                var userEmail = userDto.Email;
                var message = $"A new daily task '{dailyTask.Name}' has been assigned to you for date:{dailyTask.Date.ToShortDateString()}.For more information please log in on our system.";

                await _emailService.SendDailyTaskEmailAsync(userEmail, message);
            }

            return dailyTaskDto;
        }
        public async Task DeleteAsync(Guid id)
        {
            var room = await _dailyTaskRepository.GetDailyTaskByIdAsync(id);
            await _dailyTaskRepository.Delete(room);

        }
        public async Task<IEnumerable<DailyTaskDto>> GetAllDailyTasksAsync()
        {
            var dailyTasks = await _dailyTaskRepository.GetDailyTasksAsync();
            var dailyTaskDto = _mapp.Map<IEnumerable<DailyTaskDto>>(dailyTasks);
            return dailyTaskDto;
        }

        public async Task<DailyTaskDto> GetDailyTaskByIdAsync(Guid id)
        {
            var dailyTask = await _dailyTaskRepository.GetDailyTaskByIdAsync(id);
            var dailyTaskDto = _mapp.Map<DailyTaskDto>(dailyTask);
            return dailyTaskDto;
        }
        public async Task UpdateAsync(Guid id, DailyTaskDto dailyTaskDto)
        {
            var existingDailyTask = await _dailyTaskRepository.GetDailyTaskByIdAsync(id);

            _mapp.Map(dailyTaskDto, existingDailyTask);
            await _dailyTaskRepository.UpdateAsync(id, existingDailyTask);
        }

        public async Task<IEnumerable<DailyTaskDto>> GetDailyTasksByStatusAsync(string status)
        {
            var dailyTasks = await _dailyTaskRepository.GetDailyTasksAsync();
            var filteredTasks = dailyTasks.Where(task => task.Status == status);
            return _mapp.Map<IEnumerable<DailyTaskDto>>(filteredTasks);
        }


        public async Task<IEnumerable<DailyTaskDto>> GetDailyTasksByDateAsync(DateTime date)
        {
            var dailyTasks = await _dailyTaskRepository.GetDailyTasksAsync();
            var filteredTasks = dailyTasks.Where(task => task.Date.Date == date.Date);
            return _mapp.Map<IEnumerable<DailyTaskDto>>(filteredTasks);
        }

        public async Task<IEnumerable<DailyTaskDto>> GetDailyTasksByStaffIdAsync(Guid staffId)
        {
            var dailyTasks = await _dailyTaskRepository.GetDailyTasksAsync();
            var filteredTasks = dailyTasks.Where(task => task.StaffId == staffId);
            return _mapp.Map<IEnumerable<DailyTaskDto>>(filteredTasks);
        }

        public async Task<IEnumerable<DailyTaskDto>> GetDailyTasksByUserIdAsync(Guid userId)
        {
            var dailyTasks = await _dailyTaskRepository.GetDailyTasksAsync();
            var filteredTasks = dailyTasks.Where(task => task.Staff.UserId == userId);
            return _mapp.Map<IEnumerable<DailyTaskDto>>(filteredTasks);
        }
    }
}