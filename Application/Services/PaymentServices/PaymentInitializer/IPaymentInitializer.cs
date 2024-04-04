using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Application.Services.PaymentServices.PaymentInitializer
{
    public interface IPaymentInitializer
    {
        Task<IActionResult> InitiatePayment(string paymentMethod, decimal totalAmount, Guid billId, TempDataDictionary tempData);
    }
}
