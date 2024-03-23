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
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _pservice;
        private readonly IBillService _billService;
        private readonly IValidator<PaymentDto> _paymentValidator;
        private readonly StripeSettings _stripeSettings;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PayPalSettings _payPalSettings;



        public PaymentController(IPaymentService pservice, IBillService billService, IValidator<PaymentDto> paymentValidator,
            IOptions<StripeSettings> stripeSettings, IOptions<PayPalSettings> payPalSettings,IHttpContextAccessor context, UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _pservice = pservice;
            _billService = billService;
            _paymentValidator = paymentValidator;
            _stripeSettings = stripeSettings.Value;
            _payPalSettings = payPalSettings.Value; 
            httpContextAccessor = context;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
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
            else if (paymentDto.PaymentMethod == Domain.Enums.PaymentMethod.PayPal)
            {
                return await CreatePayPalPayment(paymentDto.TotalAmount, paymentDto.BillId);
            }
            else
            {
                await _pservice.AddPaymentAsync(paymentDto);
                return RedirectToAction("Index", "Payment");
            }
        }

            // Stripe Integration Checkout Part:
        public async Task<IActionResult> CreateCheckoutSession(string amount, Guid billId)
        {
            var billDto = await _billService.GetBillById(billId);
            if (billDto == null)
            {
                return RedirectToAction("Index", "Payment");
            }

            var currency = "eur";
            Stripe.StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            var customername = billDto.Username;
            var lineItemDescription = $"Room Type: {billDto.RoomType}, Days Spent: {billDto.DaysSpent}";

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "ideal", "card" },
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
                        Name = customername,
                        Description = lineItemDescription
                    }
                },
                Quantity = 1
            }
        },
                Mode = "payment",
                SuccessUrl = _stripeSettings.SuccessUrl,
                CancelUrl = _stripeSettings.CancelUrl
            };

            var service = new Stripe.Checkout.SessionService();
            var session = await service.CreateAsync(options);

            TempData["billId"] = billId;
            TempData["amount"] = amount;

            return Redirect(session.Url);
        }

        public async Task<IActionResult> SuccessStripe()
        {
            if (!TempData.ContainsKey("billId") || !TempData.ContainsKey("amount"))
            {
                TempData["error"] = "TempData does not contain necessary data.";
                return RedirectToAction("Index", "Payment");
            }

            var billId = (Guid)TempData["billId"];
            var amount = TempData["amount"].ToString();

            var billDto = await _billService.GetBillById(billId);
            if (billDto == null)
            {
                TempData["error"] = "Bill not found.";
                return RedirectToAction("Index", "Payment");
            }

            var paymentDto = new PaymentDto
            {
                BillId = billId,
                PaymentMethod = Domain.Enums.PaymentMethod.Stripe,
                TotalAmount = Convert.ToDecimal(amount),
                Username = billDto.Username
            };

            await _pservice.AddPaymentAsync(paymentDto);

            TempData.Remove("billId");
            TempData.Remove("amount");

            return View();
        }
        public IActionResult CancelStripe()
        {
            return View();
        }

            // PayPal Integration Checkout Part:
        private async Task<IActionResult> CreatePayPalPayment(decimal totalAmount, Guid billId)
        {
            var billDto = await _billService.GetBillById(billId);
            if (billDto == null)
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["billId"] = billId;
            TempData["totalAmount"] = totalAmount.ToString();

            var returnUrl = _payPalSettings.SuccessUrl;
            var cancelUrl = _payPalSettings.CancelUrl;

            var createdPayment = await _unitOfWork.PaypalServices.CreateOrderAsync(totalAmount, returnUrl, cancelUrl);
            var approvalUrl = createdPayment.links.FirstOrDefault(x => x.rel.ToLower() == "approval_url")?.href;

            if (!string.IsNullOrEmpty(approvalUrl))
            {
                return Redirect(approvalUrl);
            }
            else
            {
                TempData["error"] = "Failed to initiate PayPal payment.";
                return RedirectToAction("Index", "Payment");
            }
        }


        public async Task<IActionResult> SuccessPayPal(string paymentId, string token, string PayerID)
        {
            if (!TempData.ContainsKey("billId") || !TempData.ContainsKey("totalAmount"))
            {
                TempData["error"] = "TempData does not contain necessary data.";
                return RedirectToAction("Index", "Payment");
            }

            var billId = (Guid)TempData["billId"];
            var totalAmount = TempData["totalAmount"].ToString();
            var billDto = await _billService.GetBillById(billId);
            if (billDto == null)
            {
                TempData["error"] = "Bill not found.";
                return RedirectToAction("Index", "Payment");
            }

            var paymentDto = new PaymentDto
            {
                BillId = billId,
                PaymentMethod = Domain.Enums.PaymentMethod.PayPal,
                TotalAmount = decimal.Parse(totalAmount),
                Username = billDto.Username
            };

            await _pservice.AddPaymentAsync(paymentDto);

            ViewData["PaymentId"] = paymentId;
            ViewData["token"] = token;
            ViewData["payerId"] = PayerID;

            TempData.Remove("billId");
            TempData.Remove("totalAmount");
            return View();
        }
        public IActionResult CancelPayPal()
        {
            return View();
        }
      
    }
}
