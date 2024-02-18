using Application.DTOs;
using Application.Services.BillService;
using Application.Services.PaymentServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _pservice;
        private readonly IBillService _billService;

        public PaymentController(IPaymentService pservice, IBillService billService)
        {
            _pservice = pservice;
            _billService = billService; 
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var paymentdto = await _pservice.GetPaymentByIdAsync(id);
            if (paymentdto == null)
            {
                return NotFound();
            }

            return View(paymentdto);
        }

       

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var payments = await _pservice.GetAllPaymentsAsync();
            return View(payments);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var paymentDto = new PaymentDto(); 
            var bills = await _billService.GetAllBills(); 
            paymentDto.Bills = bills;

            return View(paymentDto); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentDto paymentDto)
        {
            if (ModelState.IsValid)
            {
                // Fetch the UserId from the current user's claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    // Set the UserId in the associated BillDto
                    if (paymentDto.Bills != null && paymentDto.Bills.Any())
                    {
                        foreach (var bill in paymentDto.Bills)
                        {
                            // You might not need this if UserId is already set in the DTO
                            bill.UserId = userId;
                        }
                    }

                    await _pservice.AddPaymentAsync(paymentDto);
                    return RedirectToAction(nameof(Details), new { id = paymentDto.Id });
                }
                else
                {
                    // Handle the case where UserId couldn't be retrieved from claims
                    ModelState.AddModelError("", "Failed to retrieve UserId from claims.");
                }
            }

            // If model state is not valid or UserId retrieval failed, reload necessary data and return to the view
            paymentDto.Bills = await _billService.GetAllBills();
            return View(paymentDto);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var payment = await _pservice.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, PaymentDto paymentDto)
        {
            await _pservice.UpdatePaymentAsync(id, paymentDto);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _pservice.DeletePaymentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
