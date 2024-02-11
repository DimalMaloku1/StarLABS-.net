using Application.DTOs;
using Application.Services.PaymentServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _pservice;

        public PaymentController(IPaymentService pservice)
        {
            _pservice = pservice;
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var payment = await _pservice.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var payments = await _pservice.GetAllPaymentsAsync();
            return View(payments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentDto paymentDto)
        {
            await _pservice.AddPaymentAsync(paymentDto);
            return RedirectToAction(nameof(Details), new { id = paymentDto.Id });
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
