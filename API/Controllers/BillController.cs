using System.Runtime.InteropServices.JavaScript;
using Application.DTOs;
using Application.Services.BillService;
using Application.Services.BookingServices;
using Application.Validations;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using FluentValidation;

namespace API.Controllers
{
    public class BillController : Controller
    {
        private readonly IBillService _billService;
        private readonly IValidator<BillDto> _billValidator;
        private readonly IBookingService _bookingService;
        public BillController(IBillService billService, IValidator<BillDto> billValidator, IBookingService bookingService)
        {
            _billService = billService;
            _billValidator = billValidator;
            _bookingService = bookingService;
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
                    return RedirectToAction(nameof(Index));
                }
              

             
              
                ViewBag.Bookings = await _bookingService.GetAllBookingsAsync();
                return View(billDto);
            }
        
        // [HttpPost]
        // public async Task<IActionResult> Create(PaymentDto paymentDto)
        // {
        //     var validationResult = _paymentValidator.Validate(paymentDto);
        //     if (validationResult.IsValid)
        //     {
        //         await _pservice.AddPaymentAsync(paymentDto);
        //         return RedirectToAction("Index");
        //     }
        //
        //     paymentDto.Bills = await _billService.GetAllBills();
        //     return View(paymentDto);
        // }

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
            return RedirectToAction(nameof(Details), new { billDto.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _billService.DeleteBill(id);
            return RedirectToAction(nameof(Index));
        }

       
    }
}