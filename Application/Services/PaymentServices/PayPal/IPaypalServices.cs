using PayPal.Api;

namespace Application.Services.PaymentServices.PayPal
{
    public interface IPaypalServices
    {
        Task<Payment> CreateOrderAsync(decimal amount, string returnUrl, string cancelUrl);
    }
}
