using Microsoft.AspNetCore.Mvc;
using Stripe;
using Application.Services.BillService;
using Stripe.Checkout;

namespace Application.Services.PaymentServices.Stripe
{
    public class StripeService(IBillService _billService) : IStripeService
    {
        public async Task<IActionResult> CreateCheckoutSession(decimal totalAmount, Guid billId, string successUrl, string cancelUrl, string apiKey)
        {
            var billDto = await _billService.GetBillById(billId);
            if (billDto == null)
            {
                return new NotFoundResult();
            }
            StripeConfiguration.ApiKey = apiKey;
            var customername = billDto.Username;
            var lineItemDescription = $"Room Type: {billDto.RoomType}, Days Spent: {billDto.DaysSpent}";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = ["card", "ideal"],
                LineItems =
                [
                    new() {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "eur",
                            UnitAmount = (long)(totalAmount * 100),
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = customername,
                                Description = lineItemDescription
                            }
                        },
                        Quantity = 1
                    }
                ],
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl
            };
            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return new RedirectResult(session.Url);
        }
    }
}
