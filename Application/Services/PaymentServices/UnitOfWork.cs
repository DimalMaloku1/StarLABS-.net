﻿using Microsoft.Extensions.Configuration;

namespace Application.Services.PaymentServices
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IConfiguration _configuration;

        public UnitOfWork(IConfiguration configuration)
        {
            _configuration = configuration;
            PaypalServices = new PaypalServices(_configuration);
        }
        public IPaypalServices PaypalServices { get; private set; }
    }
}
