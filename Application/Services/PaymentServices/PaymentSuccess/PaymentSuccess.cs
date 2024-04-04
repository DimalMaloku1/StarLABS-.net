using Application.DTOs;
using Application.Services.BillService;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Application.Services.PaymentServices.PaymentSuccess
{
    public class PaymentSuccess(IBillService _billService, IPaymentService _paymentService) : IPaymentSuccess
    {
        public async Task<IActionResult> ProcessSuccess(PaymentMethod paymentMethod,TempDataDictionary tempData)
        {
            string billIdKey = "billId";
            string amountKey = "amount";
            string totalAmountKey = "totalAmount";

            if (!tempData.ContainsKey(billIdKey))
            {
                tempData["error"] = "TempData does not contain necessary data.";
                return new RedirectToActionResult("Index", "Payment", null);
            }
            var billId = (Guid)tempData[billIdKey];
            var billDto = await _billService.GetBillById(billId);
            if (billDto == null)
            {
                tempData["error"] = "Bill not found.";
                return new RedirectToActionResult("Index", "Payment", null);
            }
            var paymentDto = new PaymentDto
            {
                BillId = billId,
                PaymentMethod = paymentMethod,
                Username = billDto.Username
            };
            if (paymentMethod == PaymentMethod.Stripe)
            {
                string amountKeyToUse = amountKey;
                paymentDto.TotalAmount = Convert.ToDecimal(tempData[amountKeyToUse].ToString());
                tempData.Remove(amountKeyToUse);
            }
            else if (paymentMethod == PaymentMethod.PayPal)
            {
                string totalAmountKeyToUse = totalAmountKey;
                paymentDto.TotalAmount = decimal.Parse(tempData[totalAmountKeyToUse].ToString());
                tempData.Remove(totalAmountKeyToUse);
            }
            await _paymentService.AddPaymentAsync(paymentDto);
            tempData.Remove(billIdKey);
            return new ViewResult();
        }
    }
}
