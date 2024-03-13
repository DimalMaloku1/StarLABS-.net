using Application.DTOs;
using Application.PayPal;
using Application.Services.BillService;
using Application.Services.PaymentServices;
using Application.Stripe;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace API.Controllers
{
    [AllowAnonymous]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _pservice;
        private readonly IBillService _billService;
        private readonly IValidator<PaymentDto> _paymentValidator;
        private readonly StripeSettings _stripeSettings;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;


        public PaymentController(IPaymentService pservice, IBillService billService, IValidator<PaymentDto> paymentValidator, IOptions<StripeSettings> stripeSettings, IHttpContextAccessor context, IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _pservice = pservice;
            _billService = billService;
            _paymentValidator = paymentValidator;
            _stripeSettings = stripeSettings.Value;
            httpContextAccessor = context;
            _configuration = configuration;
            _userManager = userManager;
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


        [HttpPost]
        public async Task<IActionResult> Create(PaymentDto paymentDto)
        {
            var validationResult = _paymentValidator.Validate(paymentDto);
            if (!validationResult.IsValid)
            {
                paymentDto.Bills = await _billService.GetAllBills();
                return View(paymentDto);
            }

            if (paymentDto.PaymentMethod == Domain.Enums.PaymentMethod.Stripe)
            {
                var bill = await _billService.GetBillById(paymentDto.BillId);
                if (bill != null)
                {
                    var totalAmount = bill.TotalAmount;
                    return await CreateCheckoutSession(totalAmount.ToString(), paymentDto.BillId);
                }
                else
                {
                    return RedirectToAction("Index", "Payment");
                }
            }
           
            else
            {
                await _pservice.AddPaymentAsync(paymentDto);
                return RedirectToAction("Index", "Payment");
            }
        }


        public async Task<IActionResult> CreateCheckoutSession(string amount, Guid billId)
        {
            var billDto = await _billService.GetBillById(billId);
            if (billDto == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var currency = "usd";
            var successUrl = "http://localhost:5000/Home";
            var cancelUrl = "http://localhost:5000/Home/cancel";
            Stripe.StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
        {
            new Stripe.Checkout.SessionLineItemOptions
            {
                PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                {
                    Currency = currency,
                    UnitAmount = Convert.ToInt32(amount) * 100,
                    ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Product Name",
                        Description = "Product Description"
                    }
                },
                Quantity = 1
            }
        },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl
            };

            var service = new Stripe.Checkout.SessionService();
            var session = await service.CreateAsync(options);

            var paymentDto = new PaymentDto
            {
                BillId = billId,
                PaymentMethod = Domain.Enums.PaymentMethod.Stripe,
                TotalAmount = Convert.ToDecimal(amount),
                Username = billDto.Username
            };

            await _pservice.AddPaymentAsync(paymentDto);

            return Redirect(session.Url);
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

            payment.Bills = new List<BillDto> { bill };

            return View(payment);
        }

        [HttpPost]
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
        public async Task<IActionResult> Delete(Guid id)
        {
            await _pservice.DeletePaymentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
