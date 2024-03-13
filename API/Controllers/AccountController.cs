using Application.DTOs.AccountDTOs;
using Application.Services.AccountServices;
using Application.Services.LoggingServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILoggingService _loggingService;

        public AccountController(IAccountService accountService, ILoggingService loggingService)
        {
            _accountService = accountService;
            _loggingService = loggingService;
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
                    return RedirectToAction("Index", "Home");
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
            return RedirectToAction("Index", "Home");
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

        // Other methods related to managing users can be added here




    }
}