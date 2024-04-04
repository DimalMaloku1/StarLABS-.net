using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Application.Services.PaymentServices.Stripe
{
    public interface IStripeService
    {
        Task<IActionResult> CreateCheckoutSession(decimal totalAmount, Guid billId, string successUrl, string cancelUrl, string apiKey);
    }
}
