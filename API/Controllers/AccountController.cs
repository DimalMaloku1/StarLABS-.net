using System.Threading.Tasks;
using Application.DTOs.AccountDTOs;
using Application.Services.AccountServices;
using Application.Services.LoggingServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.AccountDTOs;
using Application.Services.AccountServices;
using Application.Services.EmailServices;
using Application.Services.RazorServices;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;

namespace API.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILoggingService _loggingService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(IAccountService accountService, ILoggingService loggingService, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _accountService = accountService;
            _loggingService = loggingService;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _accountService.GetAllUsers();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.Login(loginDto);
                if (result != null && result.IsSuccess)
                {
                    return RedirectToAction("Indexx", "Home");
                }
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                ViewData["ErrorMessage"] = result.ErrorMessage;
            }
            return View(loginDto);
        }

        

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.Register(registerDto);
                if (result.IsSuccess)
                {
                    // Log user registration action
                    await _loggingService.LogActionAsync("Signed Up", null, registerDto.Email);
                        return RedirectToAction(nameof(VerifyEmail), new { email = registerDto.Email });
                }
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "An error occurred during registration.");
            }
            return View(registerDto);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction("Indexx", "Home");
        }
        [HttpGet]
        [Route("account/sendVerificationEmail")]
        public async Task<IActionResult> VerifyEmail(string email)
        {
            var user = await _accountService.GetUserByEmail(email);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> SendVerification(string email)
        {
            var encodedToken = await _accountService.GenerateEmailToken(email);
            var verificationLink = Url.Action("ConfirmEmail", "Account", new { email = email, token = encodedToken }, Request.Scheme);
            await _accountService.SendVerificationEmail(email, verificationLink);
            return RedirectToAction(nameof(Login));

        }
        [HttpGet]
        [Route("account/verify")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {

            var result = await _accountService.VerifyEmail(email, token);
            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(SuccessfulVerification));
            }
            else
            {
                return BadRequest("Email confirmation failed");
            }
        }

        [HttpGet]
        public IActionResult SuccessfulVerification()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]


        [HttpPost]
        public async Task<IActionResult> PromoteToAdmin(string email)
        {
            var result = await _accountService.PromoteUserToAdmin(email);
            if (result.IsSuccess)
                return RedirectToAction("Index", "Home");
            else
                ModelState.AddModelError(string.Empty, result.ErrorMessage);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Demote(string email)
        {
            var result = await _accountService.DemoteUser(email);
            if (result.IsSuccess)
                return RedirectToAction("Index", "Home");
            else
                ModelState.AddModelError(string.Empty, result.ErrorMessage);

            return View();
        }
        
        public async Task<IActionResult> UserRegistrationChart()
        {
            var registrationInfo = await _accountService.GetRegistrationInfo();
            return PartialView("_UserRegistrationChart", registrationInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                return RedirectToAction("Indexx", "Home");
            }
            if (signInResult.IsLockedOut)
            {
                return RedirectToAction(nameof(ForgotPassword));
            }
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new AppUser { UserName = email, Email = email };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Indexx", "Home");
        }


    }
}