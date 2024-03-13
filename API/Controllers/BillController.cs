using System.Runtime.InteropServices.JavaScript;
using Application.DTOs;
using Application.Services.BillService;
using Application.Services.BookingServices;
using Application.Validations;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Application.Services.LoggingServices;
using System.Security.Claims; // Import the namespace for LoggingService

namespace API.Controllers
{
    [Authorize]
    public class BillController : Controller
    {
        private readonly IBillService _billService;
        private readonly IValidator<BillDto> _billValidator;
        private readonly IBookingService _bookingService;
        private readonly ILoggingService _loggingService;

        public BillController(IBillService billService, IValidator<BillDto> billValidator, IBookingService bookingService, ILoggingService loggingService)
        {
            _billService = billService;
            _billValidator = billValidator;
            _bookingService = bookingService;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var bills=  await _billService.GetAllBills();
            return View(bills);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var bill = await _billService.GetBillById(id);
            return View(bill);
        }

            [HttpGet]
            public async Task<IActionResult> Create()
            {
                ViewBag.Bookings = await _bookingService.GetAllBookingsAsync();
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Create(BillDto billDto)
            {
             
                var billValidaton = _billValidator.Validate(billDto);
                if (billValidaton.IsValid){
                    await _billService.AddBill(billDto);
                    await _loggingService.LogActionAsync("Created", "Bill", User.FindFirst(ClaimTypes.Email)?.Value);
                    return RedirectToAction(nameof(Index));
                }
              

             
              
                ViewBag.Bookings = await _bookingService.GetAllBookingsAsync();
                return View(billDto);
            }
        
 
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            ViewBag.Bookings = await _bookingService.GetAllBookingsAsync();
            var billToUpdate = await _billService.GetBillById(id);
            return View(billToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> Update(BillDto billDto)
        {
            await _billService.UpdateBill(billDto);
            await _loggingService.LogActionAsync("Updated", "Bill", User.FindFirst(ClaimTypes.Email)?.Value);
            return RedirectToAction(nameof(Details), new { billDto.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _billService.DeleteBill(id);
            await _loggingService.LogActionAsync("Deleted", "Bill", User.FindFirst(ClaimTypes.Email)?.Value);
            return RedirectToAction(nameof(Index));
        }

       
    }
}