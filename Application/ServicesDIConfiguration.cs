using Application.DTOs;
using Application.Services.BillService;
using Application.Services.BookingServices;
using Application.Services.FeedbackServices;
using Application.Services.PaymentServices;
using Application.Services.RoomServices;
using Application.Services.RoomTypeServices;
using Application.Services.StaffServices;
using Application.Stripe;
using Application.Validations;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Application
{
    public class ServicesDIConfiguration
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<StaffValidator>();
            services.AddScoped<IValidator<BookingDto>,BookingValidator>();
            services.AddScoped<RoomValidator>();
            services.AddScoped<RoomTypeValidator>();
            services.AddTransient<IValidator<PaymentDto>, PaymentValidator>();
            services.AddTransient<IValidator<BillDto>, BillValidator>();
            services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));
            services.AddScoped<IRoomServices, RoomServices>();
            services.AddScoped<IRoomTypeServices, RoomTypeServices>();
            services.AddScoped<IBillService, BillService>();
        }
    }
}
