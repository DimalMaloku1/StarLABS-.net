using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.StaffServices
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;


        public StaffService(IStaffRepository staffRepository, IMapper mapper, UserManager<AppUser> userManager)
        {
            _staffRepository = staffRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<StaffDTO>> GetAllStaffAsync()
        {
            var staffEntities = await _staffRepository.GetAllStaffAsync();
            return _mapper.Map<IEnumerable<StaffDTO>>(staffEntities);
        }
        public async Task<IEnumerable<Guid>> GetAllUserNamesAsync()
        {
            return await _userManager.Users.Select(u => u.Id).ToListAsync();
        }

        public async Task<StaffDTO> GetStaffByIdAsync(Guid id)
        {
            var staffEntity = await _staffRepository.GetStaffByIdAsync(id);
            return _mapper.Map<StaffDTO>(staffEntity);
        }

        public async Task AddStaffAsync(StaffDTO staffDTO)
        {
            var staffEntity = _mapper.Map<Staff>(staffDTO);
            await _staffRepository.AddStaffAsync(staffEntity);
        }

        public async Task UpdateStaffAsync(Guid id, StaffDTO staffDTO)
        {
            var existingStaffEntity = await _staffRepository.GetStaffByIdAsync(id);

            _mapper.Map(staffDTO, existingStaffEntity);
            await _staffRepository.UpdateStaffAsync(existingStaffEntity);
        }

        public async Task DeleteStaffAsync(Guid id)
        {
            var existingStaffEntity = await _staffRepository.GetStaffByIdAsync(id);
            await _staffRepository.DeleteStaffAsync(id);
        }

        public async Task<IEnumerable<StaffDTO>> GetStaffByDepartmentAsync(string department)
        {
            var staffEntities = await _staffRepository.GetStaffByDepartmentAsync(department);
            return _mapper.Map<IEnumerable<StaffDTO>>(staffEntities);
        }

        public async Task<IEnumerable<StaffDTO>> GetStaffByPositionAsync(Guid positionId)
        {
            var staffEntities = await _staffRepository.GetStaffByPositionAsync(positionId);
            return _mapper.Map<IEnumerable<StaffDTO>>(staffEntities);
        }

        public async Task<string> GetStaffFullNameByStaffIdAsync(Guid staffId)
        {
            var staff = await _staffRepository.GetStaffByIdAsync(staffId);
            if (staff != null && staff.UserId != Guid.Empty)
            {
                var user = await _userManager.FindByIdAsync(staff.UserId.ToString());
                if (user != null)
                {
                    return $"{user.UserName} {user.UserLastname}";
                }
            }
            return "Unknown";
        }

    }
}
