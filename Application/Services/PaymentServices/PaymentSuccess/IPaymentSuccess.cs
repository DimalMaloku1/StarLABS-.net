using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Application.Services.PaymentServices.PaymentSuccess
{
    public interface IPaymentSuccess
    {
        Task<IActionResult> ProcessSuccess(Domain.Enums.PaymentMethod paymentMethod, TempDataDictionary tempData);
    }
}
