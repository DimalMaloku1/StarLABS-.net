using Application.PayPal;
using Application.Services.BillService;
using Application.Services.PaymentServices.Stripe;
using Application.Services.PaymentServices.UnitWork;
using Application.Stripe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

namespace Application.Services.PaymentServices.PaymentInitializer
{
    public class PaymentInitializer(IBillService _billService, IStripeService _stripeService, IUnitOfWork _unitOfWork,
        IOptions<PayPalSettings> payPalSettings, IOptions<StripeSettings> stripeSettings) : IPaymentInitializer
    {
        private readonly PayPalSettings _payPalSettings = payPalSettings.Value;
        private readonly StripeSettings _stripeSettings = stripeSettings.Value;

        public async Task<IActionResult> InitiatePayment(string paymentMethod, decimal totalAmount, Guid billId, TempDataDictionary tempData)
        {
            var bill = await _billService.GetBillById(billId);
            if (bill == null)
            {
                tempData["error"] = "Bill not found";
                return new RedirectToActionResult("Index", "Home", null);
            }
            if (paymentMethod == "Stripe")
            {
                var apiKey = _stripeSettings.SecretKey;
                var successUrl = _stripeSettings.SuccessUrl;
                var cancelUrl = _stripeSettings.CancelUrl;

                var result = await _stripeService.CreateCheckoutSession(bill.TotalAmount, billId, successUrl, cancelUrl, apiKey);

                if (result is RedirectResult redirectResult)
                {
                    tempData["billId"] = billId;
                    tempData["amount"] = bill.TotalAmount.ToString();
                    return new RedirectResult(redirectResult.Url);
                }
            }
            else if (paymentMethod == "PayPal")
            {
                tempData["billId"] = billId;
                tempData["totalAmount"] = totalAmount.ToString();

                var returnUrl = _payPalSettings.SuccessUrl;
                var cancelUrl = _payPalSettings.CancelUrl;
                var createdPayment = await _unitOfWork.PaypalServices.CreateOrderAsync(totalAmount, returnUrl, cancelUrl);
                var approvalUrl = createdPayment.links.FirstOrDefault(x => x.rel.Equals("approval_url", StringComparison.CurrentCultureIgnoreCase))?.href;

                if (!string.IsNullOrEmpty(approvalUrl))
                {
                    return new RedirectResult(approvalUrl);
                }
                else
                {
                    tempData["error"] = "Failed to initiate PayPal payment.";
                }
            }
            return new RedirectToActionResult("Index", "Home", null);
        }
    }
}
