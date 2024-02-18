using Application.Services.BillService;
using Application.Services.BookingServices;
using Application.Services.FeedbackServices;
using Application.Services.PaymentServices;
using Application.Services.RoomServices;
using Application.Services.RoomTypeServices;
using Application.Services.StaffServices;
using Application.Validations;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public class ServicesDIConfiguration
    {
        public static void Configure(IServiceCollection services)
        {

            services.AddScoped<IBookingService, BookingService>();

            services.AddScoped<IFeedbackService, FeedbackService>();

            services.AddScoped<IStaffService, StaffService>();

            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<IRoomServices, RoomServices>();

            services.AddScoped<IRoomTypeServices, RoomTypeServices>();
            services.AddScoped<IBillService, BillService>();


        }
    }
}
