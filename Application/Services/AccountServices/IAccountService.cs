using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core;
using Application.DTOs.AccountDTOs;
using Domain.Models;

namespace Application.Services.AccountServices
{
    public interface IAccountService
    {
        Task<Result<UserDto>> Login(LoginDto loginDto);
        Task Logout();
        Task<Result<UserDto>> Register(RegisterDto registerDto);
        Task<IEnumerable<UserDto>> GetAllUsers(); // Add this method
        Task<UserDto> GetUserById(Guid id);
        Task DeleteUser(Guid id);
        Task<UserDto> UpdateUser(UserDto user);
        Task<Result<UserDto>> PromoteUserToAdmin(string userId);
        Task<UserDto> GetUserByEmail(string email);

        Task<Result<UserDto>> DemoteUser(string userId);
        Task SendVerificationEmail(string email, string link);
        Task<string> GenerateEmailToken(string email);
        Task<Result<AppUser>>VerifyEmail(string email, string token);
        Task<List<UserRegistrationChartDto>> GetRegistrationInfo();



    }
}
