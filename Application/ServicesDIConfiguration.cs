using Application.DTOs;
using Application.Services.BillService;
using Application.Services.BookingServices;
using Application.Services.FeedbackServices;
using Application.Services.PaymentServices;
using Application.Services.RoomServices;
using Application.Services.RoomTypeServices;
using Application.Services.EmailServices;
using Application.Services.StaffServices;
using Application.Stripe;
using Application.Validations;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Services.TokenService;
using Application.Services.AccountServices;
using Application.DTOs.AccountDTOs;
using Application.Services.PositionServices;
using Application.Services.DashboardService;
using Application.Services.LoggingServices;
using Application.Services.RazorServices;

namespace Application
{
    public class ServicesDIConfiguration
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IPositionServices, PositionServices>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IValidator<BookingDto>, BookingValidator>();
            services.AddTransient<IValidator<RoomTypeDto>, RoomTypeValidator>();
            services.AddTransient<IValidator<RoomDto>, RoomValidator>();
            services.AddTransient<IValidator<StaffDTO>, StaffValidator>();
            services.AddTransient<IValidator<PaymentDto>, PaymentValidator>();
            services.AddTransient<IValidator<BillDto>, BillValidator>();
            services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));
            services.AddScoped<IRoomServices, RoomServices>();
            services.AddScoped<IRoomTypeServices, RoomTypeServices>();
            services.AddScoped<IBillService, BillService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IValidator<LoginDto>, LoginValidator>();
            services.AddScoped<IValidator<RegisterDto>, RegisterValidator>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<ILoggingService, LoggingService>();

            services.AddTransient<IRazorPartialToStringRenderer, RazorPartialToStringRenderer>();
        }
    }
}