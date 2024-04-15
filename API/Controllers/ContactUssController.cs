using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Application.Services.LoggingServices;

namespace API.Controllers
{
    [AllowAnonymous]
    public class ContactUssController : Controller
    {
        private readonly IContactUsService _contactUsService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILoggingService _loggingService;

        public ContactUssController(IContactUsService contactUsService, UserManager<AppUser> userManager, ILoggingService loggingService)
        {
            _contactUsService = contactUsService;
            _userManager = userManager;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var messages = await _contactUsService.GetAllMessagesAsync();
            return View(messages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var message = await _contactUsService.GetMessageByIdAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage(ContactUsMessage message, string name, string email)
        {
            // If user is authenticated, use their ID, otherwise create a new ContactUsMessage
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    message.UserId = user.Id;
                }
            }
            else
            {
                // Set the name and email provided by the guest
                message.Name = name;
                message.Email = email;
            }

            if (ModelState.IsValid)
            {
                await _contactUsService.AddMessageAsync(message);
                await _loggingService.LogActionAsync("Created", "ContactUsMessage", User?.Identity.Name);
                return RedirectToAction(nameof(SendMessage));
            }

            return View(message);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsReplied(Guid id)
        {
            var message = await _contactUsService.GetMessageByIdAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            message.Replied = true;

            await _contactUsService.UpdateMessageAsync(message);

            await _loggingService.LogActionAsync("Marked as Replied", "ContactUsMessage", User.FindFirst(ClaimTypes.Email)?.Value);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            var message = await _contactUsService.GetMessageByIdAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            await _contactUsService.DeleteMessageAsync(id);
            await _loggingService.LogActionAsync("Deleted", "ContactUsMessage", User.FindFirst(ClaimTypes.Email)?.Value);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult SendMessage()
        {
            return View("SendMessage", new ContactUsMessage());
        }
    }
}
