using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core;
using Application.DTOs.AccountDTOs;
using Application.Services.EmailServices;
using Application.Services.RazorServices;
using AutoMapper;
using Domain.Models;
using Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.AccountServices
{
    internal sealed class AccountService : IAccountService
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        private readonly IEmailService _emailService;
        private readonly IRazorPartialToStringRenderer _renderer;

        public AccountService(RoleManager<IdentityRole<Guid>> roleManager, SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager, IMapper mapper, DataContext dataContext, IEmailService emailService,
            IRazorPartialToStringRenderer renderer
            )
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _dataContext = dataContext;
            _emailService = emailService;
            _renderer = renderer;

        }

        public async Task<Result<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
                return Result<UserDto>.Failure("User does not exist");

            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!passwordCheck)
                return Result<UserDto>.Failure("Failed to login the user");
            if(!user.EmailConfirmed) return Result<UserDto>.Failure("Please verify your account before logging in");

            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);

            if (result.Succeeded)
            {
                var userDto = _mapper.Map<UserDto>(user);
                //userDto.Token = await _tokenService.CreateToken(user);

                return Result<UserDto>.Success(userDto);
            }

            return Result<UserDto>.Failure("Failed to login the user");
        }
        public async Task<Result<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
                return Result<UserDto>.Failure("Email is taken try another one!");

            var user = new AppUser
            {
                UserName = registerDto.UserName,
                UserLastname = registerDto.UserLastName,
                Email = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            var userCount = await _userManager.Users.CountAsync();

            if (result.Succeeded)
            {
                bool isAdmin = userCount == 1;

                var roleName = isAdmin ? "Admin" : "User";
                var role = await _roleManager.FindByNameAsync(roleName);

                if (role is not null)
                    await _userManager.AddToRoleAsync(user, roleName);

                var userDto = _mapper.Map<UserDto>(user);

               

                return Result<UserDto>.Success(userDto);
            }

            return Result<UserDto>.Failure("Failed to register the user");
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetUserById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return _mapper.Map<UserDto>(user);
        }

        public async Task DeleteUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
                await _userManager.DeleteAsync(user);
        }

        public async Task<UserDto> UpdateUser(UserDto user)
        {
            if (user == null)
                return null;

            var userToUpdate = _mapper.Map<AppUser>(user);
            if (string.IsNullOrEmpty(userToUpdate.SecurityStamp))
                userToUpdate.SecurityStamp = await _userManager.GenerateConcurrencyStampAsync(userToUpdate);
            _dataContext.Entry(userToUpdate).State = EntityState.Detached;

            await _userManager.UpdateAsync(userToUpdate);

            try
            {
                await _dataContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity == userToUpdate)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];

                            // TODO: decide which value should be written to database
                        }

                        entry.OriginalValues.SetValues(databaseValues);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "Don't know how to handle concurrency conflicts for " + entry.Metadata.Name);
                    }
                }
            }

            return user;
        }
        
        public async Task<Result<UserDto>> PromoteUserToAdmin(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<UserDto>.Failure("User not found");

            var roleExists = await _roleManager.RoleExistsAsync("Admin");
            if (!roleExists)
                return Result<UserDto>.Failure("Admin role does not exist");

            var result = await _userManager.AddToRoleAsync(user, "Admin");
            if (result.Succeeded)
                return Result<UserDto>.Success(_mapper.Map<UserDto>(user));

            return Result<UserDto>.Failure("Failed to promote user to admin");
        }

        public async Task<Result<UserDto>> DemoteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<UserDto>.Failure("User not found");

            var result = await _userManager.RemoveFromRoleAsync(user, "Admin");
            if (result.Succeeded)
                return Result<UserDto>.Success(_mapper.Map<UserDto>(user));

            return Result<UserDto>.Failure("Failed to demote user from admin");
        }

        public async Task SendVerificationEmail(string email, string verificationLink)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return;
            }
            var verifyDto = new VerifyAccountDto
            {
                Username = user.UserName,
                Email = user.Email,
                VerificationToken = verificationLink
            };
            var html = await _renderer.RenderPartialToStringAsync("_EmailVerification", verifyDto);
            await _emailService.SendVerificationEmailAsync(email, html);

        }

        public async Task<string> GenerateEmailToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var token =  "";
            if (user != null)
            {
                 token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            }
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            return encodedToken;
        }

        public async Task <Result<AppUser>> VerifyEmail(string email, string token)
        {
            if (email == null || token == null)
            {
                return Result<AppUser>.Failure("Email or token is missing");
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                 return Result<AppUser>.Failure("User not found");
            }
            var decodedTokenBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
            {
                return Result<AppUser>.Failure("Email could not be verified");
            }

            return Result<AppUser>.Success(user);
        }

        public async Task<List<UserRegistrationChartDto>> GetRegistrationInfo()
        {
            var users = await GetAllUsers();
            var registrationInfoList = new List<UserRegistrationChartDto>();

            foreach (var user in users)
            {
                
                    var registrationDto = new UserRegistrationChartDto
                    {
                        FirstName = user.UserName,
                        LastName = user.UserLastName,
                        RegistrationDate = user.RegistrationDate
                    };
                    registrationInfoList.Add(registrationDto);
                
            }

            return registrationInfoList;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;
            return _mapper.Map<UserDto>(user);
        }
    }
}
