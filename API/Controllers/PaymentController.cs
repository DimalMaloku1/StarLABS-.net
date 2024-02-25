using Application.DTOs;
using Application.PayPal;
using Application.Services.BillService;
using Application.Services.PaymentServices;
using Application.Stripe;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PayPal.Api;


namespace API.Controllers
{
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

            // Populate the Username property in PaymentDto from associated BillDto
            foreach (var payment in payments)
            {
                // Assuming GetBillById method fetches the BillDto including the Username
                var bill = await _billService.GetBillById(payment.BillId);
                if (bill != null)
                {
                    payment.Username = bill.Username;
                }
            }

            return View(payments);
}


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Retrieve the currently logged-in user's ID
            var userIdString = _userManager.GetUserId(User);

            // Convert the user ID string to Guid
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                // Handle invalid user ID
                return RedirectToAction("Index", "Home");
            }

            var paymentDto = new PaymentDto();

            // Fetch bills associated with the current user
            var userBills = await _billService.GetBillsByUser(userId);

            if (userBills.Any())
            {
                // Map the user's bills to BillDto objects
                var billDtos = userBills.Select(bill => new BillDto
                {
                    Id = bill.Id,
                    TotalAmount = bill.TotalAmount,
                    Username = bill.Username // Assuming the bill contains username information
                                             // Map other properties as needed
                });

                // Populate the Bills property of the PaymentDto
                paymentDto.Bills = billDtos.ToList();
            }
            else
            {
                // If the user has no bills, set Bills property to an empty list
                paymentDto.Bills = new List<BillDto>();
            }

            return View(paymentDto);
        }



        [HttpPost]
        public async Task<IActionResult> Create(PaymentDto paymentDto)
        {
            // Validate the PaymentDto
            var validationResult = _paymentValidator.Validate(paymentDto);
            if (!validationResult.IsValid)
            {
                // If validation fails, reload the view with validation errors
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
                    return RedirectToAction("Index", "Home");
                }
            }
            else if (paymentDto.PaymentMethod == Domain.Enums.PaymentMethod.PayPal)
            {
                var bill = await _billService.GetBillById(paymentDto.BillId);
                if (bill != null)
                {
                    var totalAmount = bill.TotalAmount;
                    return RedirectToAction("PaymentWithPaypal", new { blogId = paymentDto.BillId });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // For other payment methods (Cash, Credit, etc.), save the payment to the database
                await _pservice.AddPaymentAsync(paymentDto);
                return RedirectToAction("Index", "Home");
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
            var successUrl = "http://localhost:5000/Home/success";
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


        public IActionResult PaymentWithPaypal(string Cancel = null, string blogId = "", string PayerID = "", string guid = "")
        {
            var ClientID = _configuration.GetValue<string>("PayPal:Key");
            var ClientSecret = _configuration.GetValue<string>("PayPal:Secret");
            var mode = _configuration.GetValue<string>("PayPal:mode");
            APIContext apiContext = PaypalConfiguration.GetAPIContext(ClientID, ClientSecret, mode);
            try
            {
                string payerId = PayerID;
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = $"{this.Request.Scheme}://{this.Request.Host}/Payment/PaymentWithPaypal?";
                    var guidd = Convert.ToString((new Random()).Next(100000));
                    guid = guidd;
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, blogId);
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        PayPal.Api.Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    httpContextAccessor.HttpContext.Session.SetString("payment", createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var paymentId = httpContextAccessor.HttpContext.Session.GetString("payment");
                    var executedPayment = ExecutePayment(apiContext, payerId, paymentId as string);
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("PaymentFailed");
                    }
                    var blogIds = executedPayment.transactions[0].item_list.items[0].sku;
                    return View("PaymentSuccess");
                }
            }
            catch (Exception)
            {
                return View("PaymentFailed");
            }
            return View("SuccessView");
        }

        private PayPal.Api.Payment payment;
        private PayPal.Api.Payment ExecutePayment(PayPal.Api.APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PayPal.Api.PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new PayPal.Api.Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private PayPal.Api.Payment CreatePayment(PayPal.Api.APIContext apiContext, string redirectUrl, string blogId)
        {
            var itemList = new PayPal.Api.ItemList()
            {
                items = new List<PayPal.Api.Item>()
            };
            itemList.items.Add(new PayPal.Api.Item()
            {
                name = "Item Detail",
                currency = "USD",
                price = "1.00",
                quantity = "1",
                sku = "asd"
            });
            var payer = new PayPal.Api.Payer()
            {
                payment_method = "paypal"
            };
            var redirUrls = new PayPal.Api.RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            var amount = new PayPal.Api.Amount()
            {
                currency = "USD",
                total = "1.00",
            };
            var transactionList = new List<PayPal.Api.Transaction>();
            transactionList.Add(new PayPal.Api.Transaction()
            {
                description = "Transaction description",
                invoice_number = Guid.NewGuid().ToString(),
                amount = amount,
                item_list = itemList
            });
            this.payment = new PayPal.Api.Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return this.payment.Create(apiContext);
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
