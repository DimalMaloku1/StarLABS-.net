using Application.DTOs;
using Application.Services.BillService;
using Application.Services.PaymentServices;
using Application.Services.PaymentServices.PaymentInitializer;
using Application.Services.PaymentServices.PaymentSuccess;
using Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace API.Controllers
{
    [Authorize]
    public class PaymentController
    (IPaymentService _pservice, IBillService _billService, IValidator<PaymentDto> _paymentValidator,
     IPaymentInitializer _paymentInitializerService, IPaymentSuccess _paymentSuccessService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var payments = await _pservice.GetAllPaymentsAsync();
            foreach (var payment in payments)
            {
                var bill = await _billService.GetBillById(payment.BillId);
                if (bill != null)
                {
                    payment.Username = bill.Username;
                }
            }
            return View(payments);
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
        public async Task<IActionResult> Create(Guid? BillId)
        {
            if (BillId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var paymentDto = new PaymentDto();
            var bill = await _billService.GetBillById(BillId.Value);
            if (bill == null)
            {
                return RedirectToAction("Index", "Home");
            }
            paymentDto.BillId = bill.Id;
            paymentDto.TotalAmount = bill.TotalAmount;
            paymentDto.Username = bill.Username;
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
            var bill = await _billService.GetBillById(payment.BillId);
            if (bill == null)
            {
                return NotFound();
            }
            payment.Bills = [bill];
            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentDto paymentDto)
        {
            var validationResult = _paymentValidator.Validate(paymentDto);
            if (!validationResult.IsValid)
            {
                paymentDto.Bills = await _billService.GetAllBills();
                return View(paymentDto);
            }
            switch (paymentDto.PaymentMethod)
            {
                case PaymentMethod.Stripe:
                    return await Stripe(paymentDto.BillId);
                case PaymentMethod.PayPal:
                    return await PayPal(paymentDto.TotalAmount, paymentDto.BillId);
                default:
                    await _pservice.AddPaymentAsync(paymentDto);
                    return RedirectToAction("Index", "Payment");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PaymentDto paymentDto)
        {
            paymentDto.Bills = await _billService.GetAllBills();
            var validationResult = _paymentValidator.Validate(paymentDto);
            if (!validationResult.IsValid)
            {
                return View(paymentDto);
            }
            await _pservice.UpdatePaymentAsync(id, paymentDto);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _pservice.DeletePaymentAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private async Task<IActionResult> Stripe(Guid billId)
        {
            return await _paymentInitializerService.InitiatePayment("Stripe", 0,billId,
            (TempDataDictionary)TempData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private async Task<IActionResult> PayPal(decimal totalAmount, Guid billId)
        {
            return await _paymentInitializerService.InitiatePayment("PayPal",totalAmount, billId,
            (TempDataDictionary)TempData);
        }

        [HttpGet]
        public async Task<IActionResult> SuccessPayPal()
        {
            return await _paymentSuccessService.ProcessSuccess(PaymentMethod.PayPal,
            (TempDataDictionary)TempData);
        }

        [HttpGet]
        public async Task<IActionResult> SuccessStripe()
        {
            return await _paymentSuccessService.ProcessSuccess(PaymentMethod.Stripe,
            (TempDataDictionary)TempData);
        }

        [HttpGet]
        public IActionResult CancelStripe()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CancelPayPal()
        {
            return View();
        }
    }
}