﻿using Microsoft.Extensions.Configuration;
using PayPal.Api;

namespace Application.Services.PaymentServices.PayPal
{
    public class PaypalServices : IPaypalServices
    {
        private readonly APIContext apiContext;
        private readonly IConfiguration configuration;

        public PaypalServices(IConfiguration configuration)
        {
            this.configuration = configuration;
            var clientId = configuration["PayPal:ClientId"];
            var clientSecret = configuration["PayPal:ClientSecret"];
            var config = new Dictionary<string, string>
            {
                {"mode", "sandbox" },
                {"clientId", clientId },
                {"clientSecret", clientSecret }
            };

            var accessToken = new OAuthTokenCredential(clientId, clientSecret, config).GetAccessToken();
            apiContext = new APIContext(accessToken);
        }

        public Task<Payment> CreateOrderAsync(decimal amount, string returnUrl, string cancelUrl)
        {
            return Task.Run(() =>
            {
                var apiContext = new APIContext(new OAuthTokenCredential(configuration["PayPal:ClientId"], configuration["PayPal:ClientSecret"]).GetAccessToken());
                var itemList = new ItemList()
                {
                    items =
                    [
                        new()
                        {
                            name = "Membership Fee",
                            currency = "EUR", 
                            price = amount.ToString("0.00"), 
                            quantity = "1",
                            sku = "membership"
                        }
                    ]
                };
                var transaction = new Transaction()
                {
                    amount = new Amount()
                    {
                        currency = "EUR", 
                        total = amount.ToString("0.00"), 
                        details = new Details()
                        {
                            subtotal = amount.ToString("0.00") 
                        }
                    },
                    item_list = itemList,
                    description = "Membership Fee"
                };
                var payment = new Payment()
                {
                    intent = "sale",
                    payer = new Payer() { payment_method = "paypal" },
                    redirect_urls = new RedirectUrls()
                    {
                        return_url = returnUrl,
                        cancel_url = cancelUrl
                    },
                    transactions = [transaction]
                };
                var createdPayment = payment.Create(apiContext);
                return createdPayment;
            });
        }
    }
}
