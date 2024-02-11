using Application.Services.BookingServices;
using Application.Services.PaymentServices;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Application
{
    public class ServicesDIConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IPaymentService, PaymentService>();
        }

    }
}
